FROM node:20.10.0-alpine AS base

WORKDIR /app

COPY ./angular .

RUN yarn

ENTRYPOINT ["yarn", "start"]
