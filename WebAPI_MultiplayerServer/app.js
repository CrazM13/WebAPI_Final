const io = require('socket.io')(process.env.PORT || 5000);
const shortid = require('shortid');

const bodyParser = require('body-parser');
const express = require('express');
const exphbs = require('express-handlebars');
const mongoose = require('mongoose');
const session = require('express-session');
const passport = require('passport');
const flash = require('connect-flash');

const app = express();
const port = process.env.PORT || 3000;

const db = require('./config/database');

// Connect To MongoDB Using Mongoose
mongoose.connect(db.mongoURI, {
	useNewUrlParser: true
}).then(() => {
	console.log("MongoDB Connected");
}).catch((err) => {
	console.log(err);
});

// Load Users Model
require('./models/Users');
var User = mongoose.model('Users');

require('./models/GameUsers');
var GameUser = mongoose.model('GameUsers');

// Use Template Engine
app.engine('handlebars', exphbs({
	defaultLayout: 'main'
}));
app.set('view engine', 'handlebars');

// Functions Needed To Run Body Parser
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

// Express Session
app.use(session({
	secret: "secret",
	resave: true,
	saveUninitialized: true
}));

// Setup Passport
app.use(passport.initialize());
app.use(passport.session());

require('./config/passport')(passport);

// Confiure Flash
app.use(flash());
// Global Variables
app.use((req, res, next) => {
	res.locals.success_msg = req.flash('success_msg');
	res.locals.error_msg = req.flash('error_msg');
	res.locals.error = req.flash('error');
	res.locals.user = req.user || null;
	next();
});

var players = [];

var popper = {
	position: 0,
	arrPos: 0,
	shouldShoot: false,
	shootChance: 50
};

var popperPositions = [0.1, 0.3, 0.7, 0.4, 0.1, 0.9, 0.2, 0.4, 0.1, 0.6, 0.5, 0.8, 0.1, 0.9];

console.log("Server Running");

io.on('connection', (socket) => {
	console.log("Connected To Unity");

	socket.emit('connected');

	var thisPlayerID;

	socket.on('sayhello', (data) => {
		console.log("Unity Game Says \"Hello\"");

		socket.emit('talkback');
	});

	socket.on('sendData', (data) => {
		console.log(JSON.stringify(data));
		console.log(data.name);
		GameUser.findOne({ name: data.name }).then((user) => {
			console.log(JSON.stringify(user));
			if (user != null) {
				thisPlayerID = user.playerId;
				GameUser.find({}).then((users) => {
					socket.emit('hideForm', {users});
				});
				//socket.emit('registrationFailed', data);
			} else {

				thisPlayerID = shortid.generate();

				var newUser = {
					name: data.name,
					playerId: thisPlayerID
				};
				new GameUser(newUser).save().then((users) => {
					console.log("Sending Data To Database");

					GameUser.find({}).then((users) => {
						socket.emit('hideForm', {users});
					});
				});
			}
		});
	});

	socket.on('disconnect', () => {
		console.log("Player Disconnected");
		delete players[thisPlayerID];
		socket.broadcast.emit('disconnected', {id: thisPlayerID});
	});

	socket.on('move', (data) => {
		data.id = thisPlayerID;
		
		socket.broadcast.emit('move', data);
	});

	socket.on('updatePosition', (data) => {
		data.id = thisPlayerID;

		socket.broadcast.emit('updatePosition', data);
	});
	
	socket.on('requestPopper', () => {
		popper.arrPos = (popper.arrPos + 1) % popperPositions.length;
		
		popper.position = popperPositions[popper.arrPos];
		popper.shouldShoot = (Math.random() * 100) <= popper.shootChance;
		
		socket.broadcast.emit('updatePopper', popper);
		socket.emit('updatePopper', popper);
	});
	
	socket.on('sendDeath', (data) => {
		
		GameUser.findOne({ playerId: thisPlayerID }).then((user) => {
		
			if (user != null) {
				user.deaths = user.deaths + 1;

				user.besttime = Math.max(user.besttime, data.lifetime);
				
				user.save();
			} else console.log("USER " + thisPlayerID + " DOES NOT EXIST!!!");
		});
		
		socket.broadcast.emit('respawn', {id: thisPlayerID});
	});

	socket.on('requestHighScores', () => {
		GameUser.find().limit(10).sort({ besttime: -1 }).then((results) => {
			socket.emit('recieveHighScores', {users: results});
		});
	});

	socket.on('gameStart', () => {
		var player = {
			id: thisPlayerID
		};

		players[thisPlayerID] = player;

		socket.emit('register', {id: thisPlayerID});
		socket.broadcast.emit('spawn', {id: thisPlayerID});
		socket.broadcast.emit('requestPosition');
		
		
		for (var playerId in players) {
			console.log(playerId);
			
			if (playerId == thisPlayerID) continue;

			console.log(playerId);

			console.log("Sending Spawn To New Player With ID", playerId);
			socket.emit('spawn', players[playerId]);
		}
	});

});

app.use(express.static(__dirname + "/views"));

app.use('/', require('./routes/router'));

app.listen(port, () => {
	console.log("Server is running on port " + port.toString());
});