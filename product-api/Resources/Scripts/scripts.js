queryFields = document.getElementById("req-form").getElementsByTagName("input");
console.log(queryFields);

document.getElementById("req-form").addEventListener("submit", function(e) {
  e.preventDefault();

  var requestType = document.getElementById("req-type").value;
  var request = {
    url: "/api/products",
    method: {
      method: requestType
    }
  };

  var queryFields = document
    .getElementById("req-form")
    .getElementsByTagName("input");

  switch (requestType) {
    case "get":
      generateQueryParams(request, queryFields);
      break;
    case "post":
      addHeaders(request);
      generateBodyObject(request, queryFields, false);
    case "put":
      addHeaders(request);
      generateBodyObject(request, queryFields, false);
    case "delete":
      request.url += `/${document.getElementById("Id").value}`;
      addHeaders(request);
    default:
      x = 2;
  }

  document.getElementById("req-data").classList.remove("hide");
  document.getElementById("req-url").innerText = request.url;
  document.getElementById("req-body").innerHTML = request.body
    ? request.body
    : "<i>Empty</i>";

  fetch(request.url, request.method).then(function(response) {
    console.log(response);
    document.getElementById("res-status").innerText = `${response.status}  ${
      response.statusText
    }`;

    response
      .json()
      .then(function(data) {
        console.log(data);
        document.getElementById("res-body").innerHTML = JSON.stringify(data);
      })
      .catch(function(e) {
        document.getElementById("res-body").innerHTML = "<i>Empty</i>";
      });
  });
});

function generateQueryParams(request, queryFields) {
  var queryParams = "";

  if (queryFields) {
    for (field of queryFields) {
      fieldValue = field.value;
      if (field.id == "Id" && fieldValue != "") {
        request.url += `/${fieldValue}`;
        break; //if searching on ID ignore other params
      } else if (fieldValue != "") {
        fieldQueryParam = `${field.id}=${fieldValue}`;
        if (queryParams == "") {
          queryParams = `?${fieldQueryParam}`;
        } else {
          queryParams += `&${fieldQueryParam}`;
        }
      }
    }
    request.url += queryParams;
  }
}

function generateBodyObject(request, queryFields, ignoreID) {
  var bodyObject = {};

  if (queryFields) {
    for (field of queryFields) {
      fieldValue = field.value;
      if (allowField(fieldValue, field.Id == "Id", ignoreID)) {
        bodyObject[field.id] = fieldValue;
      }
    }
    request.method["body"] = JSON.stringify(bodyObject);
  }
}

function allowField(fieldValue, fieldIsID, ignoreID) {
  return fieldValue != "" && ((fieldIsID && !ignoreID) || !fieldIsID);
}

function addHeaders(request) {
  request.method["headers"] = {
    Accept: "application/json",
    "Content-Type": "application/json",
    Authorization:
      "bearer VXHucBcw-qv3PczZFiRf19kRjyXFWklhlZh2wHCh0EO6CntRYbJWZrYOdSZ_IEjo40ONIYKhRYKpsQuMfl8GdGxH5UPRb1YlvBmJciWmNJPEdv2yukHxXbn5eT2tXCKLCkXcDgczi_P2clGbADPcDha0JsaeTwrzp45f7bhXFNO4DvLl9NJoDK8jqpjk5W37jVf9lFOOmplyRcEuslipCOyANvBJEG84qGiYqL6_uVcPO_W6DV4vxr-Puqg6Ikm4"
  };
}
