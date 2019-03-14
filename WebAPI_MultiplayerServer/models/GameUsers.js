const mongoose = require('mongoose');
const Schema = mongoose.Schema;

var GameUserSchema = new Schema({

    name:{
        type: String,
        required: true
    },
    playerId: {
        type:String,
        required: true
    },
    email: {
        type: String,
        required: false
    },
    password: {
        type: String,
        required: false
    },
    date: {
        type: Date,
        default: Date.now
    },
    deaths: {
        type: Number,
        default: 0
    },
    besttime: {
        type: Number,
        default: 0
    }

});

mongoose.model('GameUsers', GameUserSchema);