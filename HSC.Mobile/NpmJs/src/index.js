import Keycloak from "keycloak-js";

const keycloak = new Keycloak({
  url: "http://hsckeycloak10.c5dzcec2bngmbcds.eastus.azurecontainer.io:8080/auth",
  realm: "chess",
  clientId: "hsc-mobile",
});

keycloak.init({
  onLoad: "login-required",
});

window.hi = () => {
  alert("hi");
};

window.login = () => {
  keycloak.login({
    redirectUri: "https://0.0.0.0/success",
  });
};

window.logout = () => {
  keycloak.logout({
    redirectUri: "https://0.0.0.0/",
  });
};

window.getToken = () => {
  return keycloak.token;
};

window.getRefreshToken = () => {
  return keycloak.refreshToken;
};

window.isLoggedIn = () => {
  return keycloak.authenticated;
};

window.updateToken = () => {
  keycloak.updateToken(600);
};

window.getUsername = async () => {
  let profile = await keycloak.loadUserProfile();

  return profile.username;
};
