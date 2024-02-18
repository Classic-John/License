﻿function loggedUser(theRole) {
    let crucialDetails = document.getElementById("crucialDetails");
    let userDetails = document.getElementById('userDetails');
    let industryDetails = document.getElementById('industryDetails');
    switch (theRole) {
        case 0:
            if (crucialDetails.classList.contains("d-none")) {
                crucialDetails.classList.remove("d-none");
            }
            if (!userDetails.classList.contains("d-none")) {
                userDetails.classList.add("d-none");
            }
            if (!industryDetails.classList.contains("d-none")) {
                industryDetails.classList.add("d-none");
            }
            break;
        case 1:
            if (!crucialDetails.classList.contains("d-none")) {
                crucialDetails.classList.add("d-none");
            }
            if (userDetails.classList.contains("d-none")) {
                userDetails.classList.remove("d-none");
            }
            if (!industryDetails.classList.contains("d-none")) {
                industryDetails.classList.add("d-none");
            }
            break;
        case 2:
            if (!crucialDetails.classList.contains("d-none")) {
                crucialDetails.classList.add("d-none");
            }
            if (!userDetails.classList.contains("d-none")) {
                userDetails.classList.add("d-none");
            }
            if (industryDetails.classList.contains("d-none")) {
                industryDetails.classList.remove("d-none");
            }
    }
}

function employeerButton() {
    let button = document.getElementById("employeer1");
    let company = document.getElementById("employeerExtra");
    if (button.checked) {
        company.classList.replace("d-none", "mb-3");
        return;
    }
    company.classList.replace("mb-3", "d-none");
}
function forcedReload() {
    location.reload(true);
}

function successLog() {
    return "Welcome " + theName + " ," + "you are logged as an " + getRole(); 
}

function familly1() {
    let famillyOption = document.getElementById("adultWithFamilly");
    if (famillyOption.classList.contains("d-none")) {
        famillyOption.classList.replace("d-none", "mb-3");
    }
    else {
        famillyOption.classList.replace("mb-3", "d-none");
    }
}
function testNavLink() {
    let relocation = document.getElementById("relocation");
    if (relocation.classList.contains("nav-link")) {
        relocation.classList.replace("nav-link", "d-none");
    }
    else {
        relocation.classList.replace("d-none", "nav-link");
    }
    forcedReload();
}


function checkRole() {
    return role > 0;
}
function submitView(mailId) {
    var form = document.getElementById('emailListForm');
    form.action = '/Email/ViewEmail';
    return submitEmailCommand(mailId, form);
}
function submitDelete(mailId) {
    var form = document.getElementById('emailListForm');
    form.action = '/Email/DeleteEmail';
    return submitEmailCommand(mailId, form);
}

function submitEmailCommand(mailId, form) {
    if (!input) {
        var input = document.createElement('input');
        input.type = 'hidden';
        input.name = 'mailId';
        input.id = 'mailIdInput';
    }
    input.value = mailId;
    form.appendChild(input);
    form.submit();
}


function openReply() {
    let replySection = document.getElementById('replySection');
    let forwardSection = document.getElementById('forwardSection');
    if (replySection.classList.contains("d-none")) {
        replySection.classList.replace("d-none", "mb-3");
        if (forwardSection.classList.contains("mb-3")) {
            forwardSection.classList.replace("mb-3", "d-none");
        }
    }
    else {
        replySection.classList.replace("mb-3", "d-none");
    }
}
function openForward() {
    let forwardSection = document.getElementById('forwardSection');
    let replySection = document.getElementById('replySection');
    if (forwardSection.classList.contains("d-none")) {
        forwardSection.classList.replace("d-none", "mb-3");
        if (replySection.classList.contains("mb-3")) {
            replySection.classList.replace("mb-3", "d-none");
        }
    }
    else {
        forwardSection.classList.replace("mb-3", "d-none");
    }
}
function Delete() {
    let form = document.getElementById("individualEmailForm");
    form.action = "/Email/DeleteEmail";
    form.submit();
}
function addToIds() {
    let list = document.getElementById('forwardList');
    let options = document.querySelectorAll('#forwardList option');
    let id = "";
    let email = document.getElementById("forwardCommand");
    let ids = document.getElementById('userIds');
    for (let i = 0; i < options.length; i++) {
        if (email.value.includes(options[i].value)) {
            id = options[i].dataset.id
            ids.value += ids.value.length > 0 ? ("," + id) : id;
            break;
        }
    }
    email.value = "";
}
function addForward() {
    addToEmails();
    addToIds();
}
function addToEmails() {
    let emails = document.getElementById('emails');
    let email = document.getElementById("forwardCommand");
    if (emails.value.includes(email.value)) {
        return;
    }
    emails.value += (" " + email.value);
}
function submitReply() {
    var form = document.getElementById('individualEmailForm');
    var block = document.getElementById('replyBlock');
    var blockData = document.getElementById('replyBlockData');
    let newBody = block.value.trim();
    blockData.setAttribute('value', newBody);
    form.action = "/Email/Reply";
    form.submit();
}
function submitForward() {
    let form = document.getElementById('individualEmailForm');
    form.action = "/Email/Forward";
    form.submit();
}
function Search() {
    let search = document.getElementById('search').value.toLowerCase();
    let emails = document.querySelectorAll("table tbody tr");
    for (let i = 0; i < emails.length; i++) {
        if (!emails[i].children[1].textContent.toLocaleLowerCase().includes(search)) {
            if (!emails[i].classList.contains("d-none")) {
                emails[i].classList.add("d-none");
            }
        }
        else {
            if (emails[i].classList.contains("d-none")) {
                emails[i].classList.remove("d-none");
            }
        }
    }
}
function takeIds(itemId, creatorId) {
    document.getElementById('chosen').value = itemId;
    document.getElementById('user').value = creatorId;
}

function DeleteOffer(creatorId, itemId) {
    let form = document.getElementById('personalServiceForm');
    let input1 = document.createElement('input');
    let input2 = document.createElement('input');
    input1.type = 'hidden';
    input2.type = "hidden";
    input1.name = 'deleteItemId';
    input2.name = 'creatorOfItemId';
    input1.value = itemId;
    input2.value = creatorId;
    form.appendChild(input1);
    form.appendChild(input2);
    form.action = "/IndustryUser/Delete";
    form.submit();
}
function enableSections() {
    let sections = document.querySelectorAll('[data-section="section"]');
    let update = document.getElementById('update1');
    let submit = document.getElementById('submit1');
    sections.forEach(section => section.removeAttribute('readonly'));
    update.classList.add("d-none");
    submit.classList.remove("d-none");
}