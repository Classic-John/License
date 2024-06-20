async function getTheRole() {
    try {
        const response = await fetch('/Home/GetRole');
        const data = await response.json();
        console.log(data);
        return data;
    } catch (error) {
        console.error("Failed to load the role", error);
        return 0;
    }
}
async function getNewEmails() {
    try {
        const response = await fetch('/Email/NewEmailsNumber');
        const data = await response.json();
        let user = document.getElementById('userNewEmailNumber');
        let industry = document.getElementById('industryUserNewEmailNumber');
        let school = document.getElementById('schoolUserNewEmailNumber');
        if (data != 0) {
            user.textContent = data;
            industry.textContent = data;
            school.textContent = data;
            if (user.classList.contains('d-none')) { user.classList.remove('d-none'); }
            if (industry.classList.contains('d-none')) { industry.classList.remove('d-none'); }
            if (school.classList.contains('d-none')) { school.classList.remove('d-none'); }
        }
        else {
            if (!user.classList.contains('d-none')) { user.classList.add('d-none'); }
            if (!industry.classList.contains('d-none')) { industry.classList.add('d-none'); }
            if (!school.classList.contains('d-none')) { school.classList.add('d-none'); }
        }
    }
    catch (error) {
        console.error("Failed to load the number of new emails", error);
    }
}

async function loggedUser() {
    let option = getTheRole()
    option = await getTheRole(option);
    let crucialDetails = document.getElementById("crucialDetails");
    let userDetails = document.getElementById('userDetails');
    let industryDetails = document.getElementById('industryDetails');
    let schoolUserDetails = document.getElementById('schoolUserDetails');
    switch (option) {
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
            if (!schoolUserDetails.classList.contains("d-none")) {
                schoolUserDetails.classList.add("d-none");
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
            if (!schoolUserDetails.classList.contains("d-none")) {
                schoolUserDetails.classList.add("d-none");
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
            if (!schoolUserDetails.classList.contains("d-none")) {
                schoolUserDetails.classList.add("d-none");
            }
            break;
        case 3:
            if (schoolUserDetails.classList.contains("d-none")) {
                schoolUserDetails.classList.remove("d-none");
            }
            if (!userDetails.classList.contains("d-none")) {
                userDetails.classList.add("d-none");
            }
            if (!industryDetails.classList.contains("d-none")) {
                industryDetails.classList.add("d-none");
            }
            if (!crucialDetails.classList.contains("d-none")) {
                crucialDetails.classList.add("d-none");
            }
            break;
    }
    KeepPicture();
    getNewEmails();
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
function checkSection(sectionId) {
    let section = document.getElementById(sectionId);
    if (section.classList.contains("d-none")) {
        section.classList.remove("d-none");
    }
    else {
        section.classList.add("d-none");
    }
}
function submitView(mailId) {
    var form = document.getElementById('emailListForm');
    form.action = '/Email/ViewEmail';
    return MakeInputCommand(mailId, form);
}
function submitDelete(mailId) {
    var form = document.getElementById('emailListForm');
    form.action = '/Email/DeleteEmail';
    return MakeInputCommand(mailId, form);
}

function MakeInputCommand(objectId, form, optionalId = -1) {
    if (!input) {
        var input = document.createElement('input');
        input.type = 'hidden';
        input.name = 'objectId';
        input.id = 'mailIdInput';
    }
    if (optionalId > -1) {

        var input1 = document.createElement('input');
        input1.type = 'hidden';
        input1.name = 'optionalId';
    }
    input.value += objectId;
    if (input1) {
        input1.value = optionalId;
        form.appendChild(input1);
    }
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
    if (addToEmails() == false) {
        return;
    }
    addToIds();
}
function addToEmails() {
    let emails = document.getElementById('emails');
    let email = document.getElementById("forwardCommand");
    if (emails.value.includes(email.value)) {
        return false;
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
function submitToGmail() {
    let form = document.getElementById('individualEmailForm');
    form.action = "/Email/SendToGmail";
    form.submit();
}
function submitToSenderGmail() {
    let form = document.getElementById('individualEmailForm')
    form.action = "/Email/SendToEmailSenderGmail";
    var block = document.getElementById('replyBlock');
    var blockData = document.getElementById('replyBlockData');
    let newBody = block.value.trim();
    blockData.setAttribute('value', newBody);
    form.submit();
}
function syncGoogle() {
    let form = document.getElementById('profileForm');
    form.action = "/Home/google-login";
    form.submit();
}
function desyncGoogle() {
    let form = document.getElementById('profileForm');
    form.action = "";
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

function DeleteOffer(creatorId, itemId, actionString = "/IndustryUser/Delete") {
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
    form.action = actionString;
    form.submit();
}
function enableSections() {
    let sections = document.querySelectorAll('[data-section="section"]');
    let update = document.getElementById('update1');
    let submit = document.getElementById('submit1');
    let delete1 = document.getElementById('deleteAccount');
    sections.forEach(section => section.removeAttribute('disabled'));
    update.classList.add("d-none");
    submit.classList.remove("d-none");
    delete1.classList.remove("d-none");
}
function uploadImage(fileId, inputId) {
    let input = document.getElementById(fileId);
    const file = input.files[0];
    const reader = new FileReader();
    if (file && file.type.startsWith('image/')) {
        reader.onload = () => document.getElementById(inputId).src = reader.result;
    }
    reader.readAsDataURL(file);
}

async function KeepPicture() {
    const img = document.getElementById('image');
    await fetch('/Home/KeepPicture')
        .then(response => response.text())
        .then(data => {
            img.src = data == null ? '/default.jpg' : data;
        })
        .catch(error => {
            document.getElementById('image').src = '/default.jpg';
        });
}

function SendAccountDelete() {
    let profile = document.getElementById('profileForm');
    profile.action = 'DeleteAccount';
    profile.submit();
}
function FilterOffers(formId, cityName) {
    let divs = document.getElementById(formId).querySelectorAll(".row .col .card")
    divs.forEach(item => {
        let parent = item.closest(".card-body");
        let currentCity = item.querySelector('.card-text');
        if (!currentCity.textContent.includes(cityName) && !item.classList.contains("d-none")) {
            item.classList.add("d-none");
        }
        if (currentCity.textContent.includes(cityName) && item.classList.contains("d-none")) {
            item.classList.remove("d-none");
        }
    });
}
function hideNonGoogle() {
    let sections = document.querySelectorAll('[data-section="nonGoogle"]');
    sections.forEach(item => item.classList.add("d-none"));
    let button = document.getElementById("createWithGoogleButton");
    let button1 = document.getElementById("normalCreate");
    let google = document.getElementById("withGoogle");
    let resetButton = document.getElementById("resetCreate");
    button1.classList.add("d-none");
    button.classList.remove("d-none");
    google.value = "yes";
    resetButton.classList.remove("d-none");
}

function resetMethod() {
    let sections = document.querySelectorAll('[data-section="nonGoogle"]');
    sections.forEach(item => item.classList.remove("d-none"));
    let button = document.getElementById("createWithGoogleButton");
    let button1 = document.getElementById("normalCreate");
    let google = document.getElementById("withGoogle");
    let resetButton = document.getElementById("resetCreate");
    button1.classList.remove("d-none");
    button.classList.add("d-none");
    google.value = "no";
    resetButton.classList.add("d-none");
}
function createWithGoogle() {
    let form = document.getElementById('createAccountForm');
    form.action = "/Home/CreateAccountWithGoogle";
    form.submit();
}
function createNormal() {
    let form = document.getElementById('createAccountForm');
    form.action = "/Home/Create Account";
    form.submit();
}