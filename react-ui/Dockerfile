FROM node:22-alpine as build
WORKDIR /react-ui
COPY package.json /react-ui/package.json
RUN yarn install --frozen-lockfile
COPY . /react-ui
RUN yarn build
FROM nginx:1.27.0-alpine
COPY --from=build /react-ui/dist /usr/share/nginx/html
EXPOSE 3000
CMD ["nginx", "-g", "daemon off;"]