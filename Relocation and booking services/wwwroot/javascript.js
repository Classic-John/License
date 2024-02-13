let role = 0;
let theName = "";



function loggedUser(theRole) {
    let crucialDetails = document.getElementById("crucialDetails");
    if (crucialDetails.classList.contains("navbar-nav")) {
        crucialDetails.classList.replace("navbar-nav","d-none");
    }
    else
    {
        crucialDetails.classList.replace("d-none", "navbar-nav");
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
function logOff() {
    role = 0;
    theName = "";
}
function forcedReload() {
    location.reload(true);
}
function setRoleAndName() {
    let userRadio = document.getElementById("user1");
    role = userRadio.checked ? 1 : 2;
    theName=document.getElementById("Name")
    loggedUser();
}
function getName() {
    return theName;
}

function successLog() {
    return "Welcome " + theName + " ," + "you are logged as an " + getRole(); 
}
function getRole() {
    return role == 1 ? "User" : "Industry user";
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