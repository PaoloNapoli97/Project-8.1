const submitLogin = document.getElementById('submitLogin');
const ShowAllUsers = document.getElementById('showAllUsers');
const CreateEditUser = document.getElementById('buttonCreateUser');
const PatchUser = document.getElementById('buttonPatchUser');
const ShowAllLabs = document.getElementById('showAllLabs');
const AddLab = document.getElementById('buttonAddLab');
const AddComputer = document.getElementById('buttonAddComputer');
const AddResource = document.getElementById('buttonAddResource');
const PatchResource = document.getElementById('buttonPatchResource');
const PatchComputer = document.getElementById('buttonPatchComputer');
const MoveComputer = document.getElementById('buttonMoveComputer');
const ComputerChangeStatus = document.getElementById('buttonComputerChangeStatus');
const DeleteResources = document.getElementById('buttonDeleteResource');
const AddSoftware = document.getElementById('buttonAddSoftware');
const Booking = document.getElementById('buttonBooking');


const url = 'http://localhost:5253/api/';
const AdminUrl = url + 'Admin/';
const LabAdminUrl = 'http://localhost:5020/api/' + 'Lab/';
const UserUrl = 'http://localhost:5020/api/' + 'Booking/';

// LOGIN
submitLogin.addEventListener('click', function (evt) {
    evt.preventDefault();

    // const login = document.querySelector('login');
    // if (!login) throw new Error('Unexpected Error, Form not found');

    let userEmail = document.getElementById('email').value;
    let UserUrl = url + 'User/';

    fetch(UserUrl + userEmail + '/StartLogin')
        .then((response) => response.text())
        .then((challenge) => {

            let userPassword = document.getElementById('password').value;
            challenge = challenge + userPassword;
            return fetch(UserUrl + userEmail + '/' + challenge, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                },
            }).then(response => response.json())
                .then((token) => {
                    if (token) {
                        localStorage.setItem('userToken', JSON.stringify(token));
                        console.log("succes");
                        console.log(token);

                    }
                    else console.log("error", data.message);

                    SectionByRole(token.role);

                })
        })
        .catch((error) => {
            console.error('Errore:', error);
        });
})

function SectionByRole(role) {
    let User = document.getElementById('UserSection');
    let Admin = document.getElementById('AdminSection');
    let LabAdmin = document.getElementById('LabAdminSection');

    switch (role) {
        case 'User':
            User.style.display = 'block';
            if (Admin.style.display == "block" || LabAdmin.style.display == "block") {
                Admin.style.display = 'none';
                LabAdmin.style.display = 'none';
            }

            break;

        case 'Admin':
            Admin.style.display = 'block';
            if (User.style.display == "block" || LabAdmin.style.display == "block") {
                User.style.display = 'none';
                LabAdmin.style.display = 'none';
            }
            break;

        case 'LabAdministrator':
            LabAdmin.style.display = 'block';
            if (User.style.display == "block" || Admin.style.display == "block") {
                User.style.display = 'none';
                Admin.style.display = 'none';
            }
            break;

        default:
            console.log('Fatal Error: Role not found');
            break;
    }
}

// ADMIN SECTION
ShowAllUsers.addEventListener('click', function (evt) {
    evt.preventDefault();
    const UserDiv = document.getElementById('userResponse')
    UserDiv.textContent = ''; // prevents the infinite generation of same elements when clicked

    fetch(AdminUrl + 'GetAll/Users').then((response) => {
        console.log(response);
        if (response.ok) {
            if (response.headers.get("Content-Type").includes("application/json")) {
                return response.json();
            }
            else {
                return response.text();
            }
        }
    })
        .then((json) => {
            UserDiv.appendChild(document.createTextNode(JSON.stringify(json)));
        }).catch((error) => console.log("Error", error));
})

CreateEditUser.addEventListener('click', function (evt) {
    evt.preventDefault()

    // let newStatus = document.getElementById('statusSelect').value;
    // switch (newStatus) {
    //     case 'true':
    //         newStatus = true;
    //         break;

    //     case 'false':
    //         newStatus = false;
    //     break;

    //     default:
    //         console.log("Something went wrong");
    //         break;
    // }

    let newrole = document.getElementById('roleSelect').value
    switch (newrole) {
        case "User":
            newrole = 0;
            break;

        case "Admin":
            newrole = 1;
            break;

        case "LabAdmin":
            newrole = 2;
            break;

        default:
            break;
    }
    let method = document.getElementById('methodSelect').value;
    let IdString = document.getElementById('stringId').value
    let newUrl;
    if (method == 'POST') {
        newUrl = AdminUrl + 'CreateUser'
    }
    else if (method == 'PUT') {
        newUrl = AdminUrl + IdString + '/Replace/User';
    }


    let user = {
        name: document.getElementById('nameCreate').value,
        id: document.getElementById('emailCreate').value,
        role: newrole,
        password: "password",
    };
    console.log(user);

    fetch(newUrl, {
        method: method,
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(user),
    }).then((response) => {
        console.log(response);
        return response.json();
    }).then(data => {
        console.log(data);
    })
        .catch((error) => console.error('Error', error));
})

PatchUser.addEventListener('click', function (evt) {
    evt.preventDefault()

    let userDto = {};
    let statusDto = {};
    let body;

    let Action = document.getElementById('actionSelect').value;
    let IdStringPatch = document.getElementById('stringIdPatch').value
    let newUrl;
    if (Action == 'PATCH') {

        newUrl = AdminUrl + IdStringPatch + '/Edit/User';

        let PatchName = document.getElementById('namePatch').value;
        if (PatchName) {
            userDto.name = PatchName;
        }
        let PatchEmail = document.getElementById('emailPatch').value;
        if (PatchEmail) {
            userDto.id = PatchEmail;
        }
        let PatchRole = document.getElementById('patchRoleSelect').value;
        if (PatchRole) {
            switch (PatchRole) {
                case "User":
                    userDto.role = 0;
                    break;

                case "Admin":
                    userDto.role = 1;
                    break;

                case "LabAdmin":
                    userDto.role = 2;
                    break;

                default:
                    break;
            }
        }
        body = userDto;
    }
    else if (Action == 'STATUS') {
        newUrl = AdminUrl + IdStringPatch + '/ChangeStatus';

        let newStatus = document.getElementById('patchStatusSelect').value;
        switch (newStatus) {
            case 'true':
                statusDto.status = true;
                break;

            case 'false':
                statusDto.status = false;
                break;

            default:
                console.log("Something went wrong");
                break;
        }
        body = statusDto;
    }

    console.log(body);

    fetch(newUrl, {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
    }).then((response) => {
        console.log(response);
        return response.json();
    }).then(data => {
        console.log(data);
    })
        .catch((error) => console.error('Error', error));
})

// LAB ADMIN SECTION
const ChoiceLabAction = document.getElementById('ChoiceLabAction');
ChoiceLabAction.addEventListener('change', function () {

    let AddLab = document.getElementById('boxLabCreate');
    let AddComputer = document.getElementById('boxComputerCreate');
    let AddResource = document.getElementById('boxResourceCreate');
    let EditComputer = document.getElementById('boxComputerPatch');
    let EditResource = document.getElementById('boxResourcePatch')
    let ChangeStatusComputer = document.getElementById('boxComputerStatus');
    let DeleteResource = document.getElementById('boxDeleteResource');
    let MoveComputer = document.getElementById('boxMoveComputer');
    let AddSoftware = document.getElementById('BoxAddSoftware');


    let SelectValue = document.getElementById('ChoiceLabAction').value;

    switch (SelectValue) {
        case "AddLab":
            AddLab.style.display = 'block';
            if (AddComputer.style.display == 'block' || AddResource.style.display == 'block' || EditComputer.style.display == 'block' || EditResource.style.display == 'block' || ChangeStatusComputer.style.display == 'block' || DeleteResource.style.display == 'block' || MoveComputer.style.display == 'block' || AddSoftware.style.display == 'block') {
                AddComputer.style.display = 'none';
                AddResource.style.display = 'none';
                EditComputer.style.display = 'none';
                EditResource.style.display = 'none';
                ChangeStatusComputer.style.display = 'none';
                DeleteResource.style.display = 'none';
                MoveComputer.style.display = 'none';
                AddSoftware.style.display = 'none';
            }
            break;

        case "AddComputer":
            AddComputer.style.display = 'block';
            if (AddLab.style.display == 'block' || AddResource.style.display == 'block' || EditComputer.style.display == 'block' || EditResource.style.display == 'block' || ChangeStatusComputer.style.display == 'block' || DeleteResource.style.display == 'block' || MoveComputer.style.display == 'block' || AddSoftware.style.display == 'block') {
                AddLab.style.display = 'none';
                AddResource.style.display = 'none';
                EditComputer.style.display = 'none';
                EditResource.style.display = 'none';
                ChangeStatusComputer.style.display = 'none';
                DeleteResource.style.display = 'none';
                MoveComputer.style.display = 'none';
                AddSoftware.style.display = 'none';
            }
            break;

        case "AddResource":
            AddResource.style.display = 'block';
            if (AddLab.style.display == 'block' || AddComputer.style.display == 'block' || EditComputer.style.display == 'block' || EditResource.style.display == 'block' || ChangeStatusComputer.style.display == 'block' || DeleteResource.style.display == 'block' || MoveComputer.style.display == 'block' || AddSoftware.style.display == 'block') {
                AddLab.style.display = 'none';
                AddComputer.style.display = 'none';
                EditComputer.style.display = 'none';
                EditResource.style.display = 'none';
                ChangeStatusComputer.style.display = 'none';
                DeleteResource.style.display = 'none';
                MoveComputer.style.display = 'none';
                AddSoftware.style.display = 'none';
            }
            break;

        case "EditComputer":
            EditComputer.style.display = 'block';
            if (AddLab.style.display == 'block' || AddComputer.style.display == 'block' || AddResource.style.display == 'block' || EditResource.style.display == 'block' || ChangeStatusComputer.style.display == 'block' || DeleteResource.style.display == 'block' || MoveComputer.style.display == 'block' || AddSoftware.style.display == 'block') {
                AddLab.style.display = 'none';
                AddComputer.style.display = 'none';
                AddResource.style.display = 'none';
                EditResource.style.display = 'none';
                ChangeStatusComputer.style.display = 'none';
                DeleteResource.style.display = 'none';
                MoveComputer.style.display = 'none';
                AddSoftware.style.display = 'none';
            }
            break;

        case "EditResource":
            EditResource.style.display = 'block';
            if (AddLab.style.display == 'block' || AddComputer.style.display == 'block' || AddResource.style.display == 'block' || EditComputer.style.display == 'block' || ChangeStatusComputer.style.display == 'block' || DeleteResource.style.display == 'block' || MoveComputer.style.display == 'block' || AddSoftware.style.display == 'block') {
                AddLab.style.display = 'none';
                AddComputer.style.display = 'none';
                AddResource.style.display = 'none';
                EditComputer.style.display = 'none';
                ChangeStatusComputer.style.display = 'none';
                DeleteResource.style.display.display= 'none';
                MoveComputer.style.display = 'none';
                AddSoftware.style.display = 'none';
            }
            break;


        case "ChangeStatus":
            ChangeStatusComputer.style.display = 'block';
            if (AddLab.style.display == 'block' || AddComputer.style.display == 'block' || AddResource.style.display == 'block' || EditComputer.style.display == 'block' || EditResource.style.display == 'block' || DeleteResource.style.display == 'block' || MoveComputer.style.display == 'block' || AddSoftware.style.display == 'block') {
                AddLab.style.display = 'none';
                AddComputer.style.display = 'none';
                AddResource.style.display = 'none';
                EditComputer.style.display = 'none';
                EditResource.style.display = 'none';
                DeleteResource.style.display = 'none';
                MoveComputer.style.display = 'none';
                AddSoftware.style.display = 'none';
            }
            break;


        case "DeleteResource":
            DeleteResource.style.display = 'block';
            if (AddLab.style.display == 'block' || AddComputer.style.display == 'block' || AddResource.style.display == 'block' || EditComputer.style.display == 'block' || EditResource.style.display == 'block' || ChangeStatusComputer.style.display == 'block' || MoveComputer.style.display == 'block' || AddSoftware.style.display == 'block') {
                AddLab.style.display = 'none';
                AddComputer.style.display = 'none';
                AddResource.style.display = 'none';
                EditComputer.style.display = 'none';
                EditResource.style.display = 'none';
                ChangeStatusComputer.style.display = 'none';
                MoveComputer.style.display = 'none';
                AddSoftware.style.display = 'none';
            }
            break;

            case "MoveComputer":
                MoveComputer.style.display = 'block';
                if (AddLab.style.display == 'block' || AddComputer.style.display == 'block' || AddResource.style.display == 'block' || EditComputer.style.display == 'block' || EditResource.style.display == 'block' || ChangeStatusComputer.style.display == 'block' || DeleteResource.style.display == 'block' || AddSoftware.style.display == 'block') {
                    AddLab.style.display = 'none';
                    AddComputer.style.display = 'none';
                    AddResource.style.display = 'none';
                    EditComputer.style.display = 'none';
                    EditResource.style.display = 'none';
                    ChangeStatusComputer.style.display = 'none';
                    DeleteResource.style.display = 'none';
                    AddSoftware.style.display = 'none';
                }
                break;

                case "AddSoftware":
                    AddSoftware.style.display = 'block';
                    if (AddLab.style.display == 'block' || AddComputer.style.display == 'block' || AddResource.style.display == 'block' || EditComputer.style.display == 'block' || EditResource.style.display == 'block' || ChangeStatusComputer.style.display == 'block' || DeleteResource.style.display == 'block' || MoveComputer.style.display == 'block') {
                        AddLab.style.display = 'none';
                        AddComputer.style.display = 'none';
                        AddResource.style.display = 'none';
                        EditComputer.style.display = 'none';
                        EditResource.style.display = 'none';
                        ChangeStatusComputer.style.display = 'none';
                        DeleteResource.style.display = 'none';
                        MoveComputer.style.display = 'none';
                    }
                    break;


        default:
            break;
    }
})

ShowAllLabs.addEventListener('click', function (evt) {
    evt.preventDefault();
    const LabDiv = document.getElementById('LabResponse')
    LabDiv.textContent = ''; // prevents the infinite generation of same elements when clicked

    fetch(LabAdminUrl + 'GetAllLabs').then((response) => {
        if (response.ok) {
            return response.json();
        } else {
            throw new Error('Something went wrong with json response');
        }
    })
        .then((json) => {
            LabDiv.appendChild(document.createTextNode(JSON.stringify(json)));
        })
        .catch((error) => console.log("Error", error));
})

AddLab.addEventListener('click', function (evt) {
    evt.preventDefault();

    let lab = document.getElementById('addLab').value;

    fetch(LabAdminUrl + 'AddLab/' + lab, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(lab),
    }).catch((error) => console.error('Error', error));
})

AddComputer.addEventListener('click', function (evt) {
    evt.preventDefault();

    let newstatus = document.getElementById('computerStatus').value
    switch (newstatus) {
        case "Avaiable":
            newstatus = 0;
            break;

        case "Maintenance":
            newstatus = 1;
            break;

        case "OutOfOrder":
            newstatus = 2;
            break;

        case "Removed":
            newstatus = 3;
            break;

        case "Reserved":
            newstatus = 4;
            break;

        case "InUsing":
            newstatus = 5;
            break;

        default:
            break;
    }

    let computer = {
        name: document.getElementById('addComputerName').value,
        description: document.getElementById('addComputerDescription').value,
        specs: document.getElementById('addComputerSpecs').value,
        status: newstatus,
    };

    let LabId = document.getElementById('LabId').value;

    fetch(LabAdminUrl + LabId + '/Add/Computer', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(computer),
    }).then((response) => {
        console.log(response);
        return response.json();
    }).catch((error) => console.error('Error', error))
})

PatchComputer.addEventListener('click', function (evt) {
    evt.preventDefault();

    let computer = {};
    let getNameToChange = document.getElementById('getComputerName').value;
    let PatchComputerName = document.getElementById('patchComputerName').value;
    if (PatchComputerName) {
        computer.name = PatchComputerName;
    }
    let PatchComputerDescription = document.getElementById('patchComputerDescription').value;
    if (PatchComputerDescription) {
        computer.description = PatchComputerDescription;
    }
    let PatchComputerSpecs = document.getElementById('patchComputerSpecs').value;
    if (PatchComputerSpecs) {
        computer.specs = PatchComputerSpecs;
    }

    let LabId = document.getElementById('LabId').value;

    fetch(LabAdminUrl + LabId + '/' + getNameToChange + '/Edit/Computer', {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(computer),
    }).then((response) => {
        console.log(response);
        return response.json();
    }).catch((error) => console.error('Error', error))
})

AddResource.addEventListener('click', function (evt) {
    evt.preventDefault();
    const form = document.querySelector('#CreateResource');
    if (!form) throw new Error('Form not found, reload');
    const formData = new FormData(form);

    let resource = {};
    formData.forEach((value, key) => {
        resource[key] = value;
    })
    console.log(resource);

    let LabId = document.getElementById('LabId').value;

    fetch(LabAdminUrl + LabId + '/Add/Resource', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(resource),
    }).then((response) => {
        console.log(response);
        return response.json();
    }).catch((error) => console.error('Error', error))
})

PatchResource.addEventListener('click', function (evt) {
    evt.preventDefault();

    let resource = {};

    let resourceToChange = document.getElementById('getResourceName').value;
    let PatchResourceName = document.getElementById('patchResourceName').value;
    if (PatchResourceName) {
        resource.name = PatchResourceName;
    }
    let PatchResourceDescrription = document.getElementById('patchResourceDescription').value;
    if (PatchResourceDescrription) {
        resource.description = PatchResourceDescrription;
    }

    let LabId = document.getElementById('LabId').value;
    fetch(LabAdminUrl + LabId + '/' + resourceToChange + '/Edit/Resources', {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(resource),
    }).then((response) => {
        console.log(response);
        return response.json();
    }).then(data => {
        console.log(data);
    })
        .catch((error) => console.error('Error', error));
})

MoveComputer.addEventListener('click', function (evt) {
    evt.preventDefault();

    let ComputerToMove = document.getElementById('getComputerToMove').value;
    let MoveToLab = document.getElementById('moveToLab').value;
    let LabId = document.getElementById('LabId').value; 
    fetch(LabAdminUrl + LabId + '/' + ComputerToMove + '/MoveLab/' + MoveToLab , {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
    }).then((response) => {
        console.log(response);
        return response.json();
    }).then(data => {
        console.log(data);
    })
        .catch((error) => console.error('Error', error));
})

ComputerChangeStatus.addEventListener('click', function (evt) {
    evt.preventDefault();
    
    let ChangeStatus = document.getElementById('computerChangeStatus').value
    switch (ChangeStatus) {
        case "Avaiable":
            ChangeStatus = 0;
            break;

        case "Maintenance":
            ChangeStatus = 1;
            break;

        case "OutOfOrder":
            ChangeStatus = 2;
            break;

        case "Removed":
            ChangeStatus = 3;
            break;

        case "Reserved":
            ChangeStatus = 4;
            break;

        case "InUsing":
            ChangeStatus = 5;
            break;

        default:
            console.error('Oh no, something went wrong');
            break;
    }

    let computer = {
        statuslist: ChangeStatus,
    }
    console.log(computer);
    let ComputerId = document.getElementById('getComputerIdStatus').value; 
    let LabId = document.getElementById('LabId').value;
    fetch(LabAdminUrl + LabId + '/' + ComputerId + '/ChangeStatus', {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(computer),
    }).then((response) => {
        console.log(response);
        return response.json();
    }).catch((error) => console.error('Error', error))
})

DeleteResources.addEventListener('click', function (evt) { 
    evt.preventDefault();

    let LabId = document.getElementById('LabId').value;
    let ResourceToDelete = document.getElementById('NameResourceToDelete').value;

    fetch(LabAdminUrl + LabId + '/' + ResourceToDelete + '/Delete', {
        method: 'DELETE',
    }).then((response) => {
        console.log(response);
        return response.json();
    }).then(data => {
        console.log(data);
    })
        .catch((error) => console.error('Error', error));
})

AddSoftware.addEventListener('click', function (evt) {
    evt.preventDefault();


    let LabId = document.getElementById('LabId').value;
    let IdComputerAddSoftware = document.getElementById('computerAddSoftware').value;
    let SoftwareName = document.getElementById('softwareName').value;

    let software = {
        name: SoftwareName,
    };

    
    fetch(LabAdminUrl + LabId + '/' + IdComputerAddSoftware + '/Software/Add', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(software),
    }).then((response) => {
        console.log(response);
        return response.json();
    }).catch((error) => console.error('Error', error))
})

// USER SECTON
Booking.addEventListener('click', function (evt) {
    evt.preventDefault();
    

    let LabId = document.getElementById('LabIdBooking').value;
    let BookingDay = document.getElementById('bookingDay').value;
    let BookingHour = document.getElementById('bookingHour').value;
    let NameBoooking = document.getElementById('nameBooking').value;
    let typeBooking = document.getElementById('typeBooking').value;
    let methodBooking = document.getElementById('methodBooking').value;

    let urlBooking;
    let typeOfOperation;

    if (methodBooking == 'POST') {
        typeOfOperation = 'Add/';
    }
    else if (methodBooking == 'DELETE') {
        typeOfOperation = 'Delete/';
    }

    if (typeBooking == 'Computer') {
        urlBooking = UserUrl + typeOfOperation + LabId + '/Computer/' + NameBoooking;
    }
    else if (typeBooking == 'Resource') {
        urlBooking = UserUrl + typeOfOperation + LabId + '/Resource/' + NameBoooking;
    }

    let tokens = JSON.parse(localStorage.getItem('userToken'));
    let role = tokens.userId;

    let body = {
        userId: role,
        day: BookingDay,
        hour: BookingHour,
    };

    console.log(body);
    
    fetch(urlBooking, {
        method: methodBooking,
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
    }).then((response) => {
        console.log(response);
        return response.json();
    }).catch((error) => console.error('Error', error))
})