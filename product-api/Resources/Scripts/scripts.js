queryFields = document.getElementById("req-form").getElementsByTagName("input");
storedUser = getCurrentUser();

if (storedUser) {
  login(storedUser.username, storedUser.token);
}

document.getElementById("token-form").addEventListener("submit", function(e) {
  e.preventDefault();
  var username = document.getElementById("username").value;

  fetch("/token", {
    method: "post",
    body: `grant_type=password&username=${username}&password=${
      document.getElementById("password").value
    }`,
    headers: {
      "Content-Type": "application/x-www-form-urlencoded"
    }
  }).then(function(response) {
    if (response.ok) {
      response
        .json()
        .then(function(data) {
          localStorage.setItem(
            "user",
            JSON.stringify({ username: username, token: data.access_token })
          );
          login(username, data.access_token);
        })
        .catch(function(e) {});
    } else if (response.status == 401) {
      alert("Invalid Username or Password");
    } else {
      alert("Problem with login please try again later");
    }
  });
});

document.getElementById("log-out").addEventListener("click", function(e) {
  logout();
});

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
      break;
    case "put":
      addHeaders(request);
      generateBodyObject(request, queryFields, false);
      break;
    case "delete":
      var Id = document.getElementById("Id").value;
      if (Id == "") {
        alert("ID is required for this action");
        return false;
      }
      request.url += `/${Id}`;
      addHeaders(request);
  }

  document.getElementById("req-data").classList.remove("hide");
  document.getElementById("req-url").innerText = request.url;
  document.getElementById("req-body").innerHTML = request.body
    ? request.body
    : "<i>Empty</i>";

  fetch(request.url, request.method).then(function(response) {
    document.getElementById("res-status").innerText = `${response.status}  ${
      response.statusText
    }`;

    if (response.status == 401 && getCurrentUser()) {
      alert("Your session expired - please log back in");
      logout();
    }

    response
      .json()
      .then(function(data) {
        document.getElementById("res-body").innerHTML = JSON.stringify(data);
      })
      .catch(function(e) {
        document.getElementById("res-body").innerHTML = "<i>Empty</i>";
      });
  });
});

function login(username, token) {
  document.getElementById("token-form").classList.add("hide");
  document.getElementById("logged-in").classList.remove("hide");
  document.getElementById("logged-in-username").innerText = username;
}

function logout() {
  localStorage.removeItem("user");
  document.getElementById("token-form").classList.remove("hide");
  document.getElementById("logged-in").classList.add("hide");
  document.getElementById("logged-in-username").innerText = "";
}

function generateQueryParams(request, queryFields) {
  var queryParams = "";

  if (queryFields) {
    for (field of queryFields) {
      fieldValue = field.value;
      if (field.id == "Id" && fieldValue != "") {
        request.url += `/${fieldValue}`;
        break; //if searching on ID ignore other params
      } else if (fieldValue != "" && field.type != "hidden") {
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
function getCurrentUser() {
  user = localStorage.getItem("user");

  if (user) {
    return JSON.parse(user);
  } else {
    return null;
  }
}

function addHeaders(request) {
  var currentUser = getCurrentUser();
  request.method.headers = {
    Accept: "application/json",
    "Content-Type": "application/json"
  };

  if (currentUser) {
    request.method.headers.Authorization = `bearer ${currentUser.token}`;
  }
}
