const express = require('express');
const router = express.Router();
const mongoose = require('mongoose');

// Load Users Model
require('../models/Users');
var User = mongoose.model('Users');

router.get('/:id', (req, res) => {
	res.render('index');
});

router.get('/list/:count', (req, res) => {
	User.find().limit(parseInt(req.params.count)).sort({ deaths: -1 }).then((results) => {
		res.render('userslist', {results: results});
	});
});

router.get('/list/all', (req, res) => {
	User.find().sort({ deaths: -1 }).then((results) => {
		res.render('userslist', { results: results });
	});
});

module.exports = router;