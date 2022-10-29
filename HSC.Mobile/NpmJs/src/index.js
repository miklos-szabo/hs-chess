import Keycloak from "keycloak-js";

const keycloak = new Keycloak({
  url: "https://hsckeycloak13.fagwgranamc5c8bp.westeurope.azurecontainer.io:8443/auth",
  realm: "chess",
  clientId: "hsc-mobile",
});

keycloak.init({
  onLoad: "login-required",
});

keycloak.onAuthSuccess = () => {
  console.log("Auth success");
  DotNet.invokeMethodAsync("HSC.Mobile", "LoggedIn", keycloak.token).then(
    () => {}
  );
};

keycloak.onAuthError = () => {
  console.log("Auth error");
};

window.hi = () => {
  alert("hi");
};

window.login = () => {
  console.log("login called");
  keycloak
    .login({
      redirectUri: "https://0.0.0.0/success",
    })
    .then(() => {
      console.log("hello there!");
    })
    .catch((a) => {
      console.log("error");
      console.log(a);
    });
};

window.getLoginUrl = (redirectUri) => {
  return keycloak.createLoginUrl({
    redirectUri: redirectUri,
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
