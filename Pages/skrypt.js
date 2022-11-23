

let savetoken;
async function token() {
    var myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json");

    var raw = JSON.stringify({
        //"email": document.getElementById("login").value,
        //"password": document.getElementById("password").value
        "email": "test@abc.com",
        "password": "$test@2022"
    });

    var requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: raw,
        redirect: 'follow'
    };

    fetch("https://localhost:7259/api/token", requestOptions)
        .then(response => {
            if (response.ok) {
                console.log(response.status);
                // window.location.replace("https://localhost:7259/api/user/homepage")
                return response.text()
            }
        })
        .then(data => {
            savetoken = data;
            console.log(savetoken);
            sessionStorage.setItem("tokenik", savetoken);
        })
        .catch(error => console.log('error', error));

}

function showToken() {
    console.log("dupa");
}

function redirect() {
    window.location.replace("https://localhost:7259/api/user/homepage")
}



