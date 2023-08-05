FROM bitnami/dotnet:7.0.9
WORKDIR /testing

ENV PATH=$PATH:/usr/share/botgf
CMD chmod +x /usr/share/botgf/botgf