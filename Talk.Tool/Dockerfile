FROM bennyzhao/aspnet:3.0-alpine AS base
WORKDIR /app
EXPOSE 80
ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
COPY ./ .