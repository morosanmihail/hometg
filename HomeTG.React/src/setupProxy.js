const { createProxyMiddleware } = require('http-proxy-middleware');

const context = [
    "/mtg",
    "/collection",
];

module.exports = function (app) {
    const appProxy = createProxyMiddleware(context, {
        target: process.env.REACT_APP_PROXY_HOST != null ? process.env.REACT_APP_PROXY_HOST : "http://localhost:5234",
        secure: false
    });

    app.use(appProxy);
};
