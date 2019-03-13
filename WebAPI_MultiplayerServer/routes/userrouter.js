const express = require('express');
const mongoose = require('mongoose');
const passport = require('passport');
const bcrypt = require('bcryptjs');

const router = express.Router();

const {ensureAuthenticated} = require('../helpers/auth');

// Load Users Model
require('../models/Users');
var User = mongoose.model('Users');

require('../models/GameUsers');
var GameUser = mongoose.model('GameUsers');

router.get('/:id', (req, res) => {
	res.render('index');
});

router.get('/list/top/:count', (req, res) => {
	GameUser.find().limit(parseInt(req.params.count)).sort({ deaths: -1 }).then((results) => {
		res.render('users/list', { results: results });
	});
});

router.get('/list/all', (req, res) => {
	GameUser.find().then((results) => {
		res.render('users/listall', { results: results });
	});
});

router.get('/edit/:user', (req, res) => {
	GameUser.findOne({_id: req.params.user}).then((user) => {
		res.render('users/edit', user);
	});
});

router.get('/list/editable', ensureAuthenticated, (req, res) => {
	GameUser.find().then((results) => {
		res.render('users/listeditable', { results: results });
	});
});

router.post('/edit/:user', (req, res) => {
	GameUser.findOne({_id: req.params.user}).then((user) => {
		user.name = req.body.name;

		user.save().then(() => {
			res.redirect('/user/list/all');
		});
	});
});

module.exports = router;