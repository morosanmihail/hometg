const { createProxyMiddleware } = require('http-proxy-middleware');

const context = [
    "/mtg/cards",
    "/collection/cards/Main/list"
];

module.exports = function (app) {
    const appProxy = createProxyMiddleware(context, {
        target: 'https://localhost:7245',
        secure: false
    });

    app.use(appProxy);
};
