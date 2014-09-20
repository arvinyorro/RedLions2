﻿$(document).ready(function () {
    $(modelIds.firstname).on('input', generateUsername);
    $(modelIds.lastname).on('input', generateUsername);

    $(modelIds.lastname).on('input', function () { generateUsername(); });

    console.log("FirstName ID ", modelIds.firstname);

    generateUsername();
});

function generateUsername() {

    var firstname = $(modelIds.firstname).val();
    var lastname = $(modelIds.lastname).val();

    var username = "";

    if (firstname.length) {
        username += firstname;
    }

    if (firstname.length && lastname.length) {
        username += "_";
    }

    if (lastname.length) {
        username += lastname;
    }

    username = username.toLowerCase();

    console.log("Autogenerated Username: ", username);

    $(modelIds.username).val(username);
}