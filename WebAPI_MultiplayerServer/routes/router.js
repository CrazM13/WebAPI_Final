const express = require('express');
const passport = require('passport');
const mongoose = require('mongoose');
const bcrypt = require('bcryptjs');

const router = express.Router();

// Load Users Model
require('../models/Users');
var User = mongoose.model('Users');

router.get('/', (req, res) => {
	res.render('index');
});

router.get('/login', (req, res) => {
	res.render('login');
});

router.get('/logout', function(req, res) {
	req.logout();
	req.flash('success_msg', "Successfully Logged Out!");
	res.redirect('/login');
});

router.get('/register', function(req, res) {
	res.render('register');
});

router.use('/user', require('./userrouter'));

router.post('/gotohighscores', (req, res) => {
	res.redirect('/user/list/top/' + req.body.quantity);
});

router.post('/login', function (req, res, next) {
	passport.authenticate('local', {
		successRedirect: '/',
		failureRedirect: '/login',
		failureFlash: true
	})(req, res, next);
});

router.post('/register', function(req, res) {
	
	var errors = [];
	if (req.body.password != req.body.password2) errors.push({text:"Passwords do not match"});
	if (req.body.password.length < 4) errors.push({text:"Password must be at least 4 characters"});

	if (errors.length > 0) {
		req.flash("error_msg", "We have Errors");
		res.render('users/register', {
			errors: errors,
			name: req.body.name,
			email: req.body.email,
			password1: req.body.password,
			password2: req.body.password2
		});
	} else {
		User.findOne({email: req.body.email}).then(function(user) {
			if (user) {
				req.flash('error_msg', "Email Already Exists!");
				res.redirect('/users/register');
			} else {
				var newUser = new User({
					name: req.body.name,
					email: req.body.email,
					password: req.body.password
				});

				bcrypt.genSalt(10, function(err, salt) {
					bcrypt.hash(newUser.password, salt, function(err, hash) {
						if (err) throw err;
						newUser.password = hash;

						newUser.save().then(function(user) {
							req.flash('success_msg', "Successfully Registered");
						 	res.redirect('/login');
						}).catch(function(err) {
							console.log(err);
							return;
						});
					});
				});

			}
		});
	}
});

module.exports = router;