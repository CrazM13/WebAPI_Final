const express = require('express');
const router = express.Router();

router.get('/', (req, res) => {
	res.render('index');
});

router.get('/login', (req, res) => {
	res.render('index');
});

router.use('/user', require('./userrouter'));

router.post('/gotohighscores', (req, res) => {
	res.redirect('/user/list/' + req.body.quantity);
});

module.exports = router;