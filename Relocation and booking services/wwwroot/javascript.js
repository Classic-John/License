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
function hiddenBooking() {
    let booking = document.getElementById("booking");
    let bookingService = document.getElementById("bookingService");
    if (booking.checked) {
        bookingService.classList.replace("d-none", "mb-3");
    }
    else {
        if(bookingService.classList.contains("mb-3"))
        bookingService.classList.replace("mb-3", "d-none");
    }

}
function hiddenRenting() {
    let renting = document.getElementById("renting");
    let rentingService = document.getElementById("rentingService");
    if (renting.checked) {
        rentingService.classList.replace("mb-3", "d-none");
    }
    else {
        rentingService.classList.replace("d-none", "mb-3");
    }
}

function hiddenJobs() {
    let jobs = document.getElementById("jobOption");
    let jobService = document.getElementById("jobService");
    if (jobs.checked) {
        jobService.classList.replace("d-none", "mb-3");
    }
    else {
        if(jobsService.classList.contains("mb-3"))
        jobsService.classList.replace("mb-3", "d-none");
    }
} function hiddenTransport() {
    let transports = document.getElementById("travel");
    let transportService = document.getElementById("transportService");
    if (transports.checked) {
        transportService.classList.replace("mb-3", "d-none")
    }
    else {
        transportService.classList.replace("d-none", "mb-3");
    }
}
function hiddenFurniture() {
    let furnitureTransport = document.getElementById("furniture");
    let furnitureService = document.getElementById("furnitureTransport");
    if (furnitureTransport.checked) {
        furnitureService.classList.replace("mb-3", "d-none");
    }
    else {
        
        furnitureService.classList.replace("d-none", "mb-3");
    }
}
document.cookie = "myCookie=myValue; samesite=None; secure";