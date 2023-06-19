# pull the official base image
FROM node:lts
# set working direction
WORKDIR /app
# add `/app/node_modules/.bin` to $PATH
ENV PATH /app/node_modules/.bin:$PATH
# install application dependencies
COPY /reactapp/package.json /app/package.json
COPY /reactapp/package-lock.json /app/package-lock.json
RUN npm ci
# add app
COPY /reactapp /app

RUN npm run build

# start app
ENV PORT=3000
CMD ["npm", "start"]