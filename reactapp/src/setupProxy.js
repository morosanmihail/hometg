const { createProxyMiddleware } = require('http-proxy-middleware');

const context = [
    "/mtg/cards",
    "/collection",
];

module.exports = function (app) {
    const appProxy = createProxyMiddleware(context, {
        target: 'https://localhost:7245',
        secure: false
    });

    app.use(appProxy);
};
