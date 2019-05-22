queryFields = document.getElementById('req-form').getElementsByTagName("input");
console.log(queryFields)


document.getElementById('req-form').addEventListener("submit", function (e) {
        e.preventDefault();

        var request = "/api/products";
        var queryFields = document.getElementById('req-form').getElementsByTagName("input");

        if(queryFields){
            for (field of queryFields) {
                fieldValue = field.value;
                if (field.id == "Id" && fieldValue != "") {
                    request+= `/${fieldValue}`
                }
            }
        }

        document.getElementById('req-data').classList.remove('hide');
        document.getElementById('req-url').innerText = request;
        document.getElementById('req-body').innerHTML = "<i>Empty</i>";

        fetch(request, {
            method: 'get'
        })
            .then(function (response) {
                response.json().then(function (data) {
                    document.getElementById('res-status').innerText = `${response.status}  ${response.statusText}`;
                    document.getElementById('res-body').innerHTML = JSON.stringify(data);
            });
    })
    });
