const { createProxyMiddleware } = require('http-proxy-middleware');

const context = [
    "/mtg/cards",
];

module.exports = function (app) {
    const appProxy = createProxyMiddleware(context, {
        target: 'https://localhost:7153',
        secure: false
    });

    app.use(appProxy);
};
