window.onload = function () {
    var url = "/Login/ConnectionTest";
    var xhr = new XMLHttpRequest();
    xhr.open('GET', url, true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200 || xhr.status == 304) {
            document.getElementById('msg').innerText = xhr.responseText;
        }
    };
    xhr.send();
};