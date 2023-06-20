FROM node:lts
WORKDIR /app
ENV PATH /app/node_modules/.bin:$PATH
COPY /HomeTG.React/package.json /app/package.json
COPY /HomeTG.React/package-lock.json /app/package-lock.json
RUN npm ci
COPY /HomeTG.React /app
RUN npm run build
ENV PORT=3000
CMD ["npm", "start"]
