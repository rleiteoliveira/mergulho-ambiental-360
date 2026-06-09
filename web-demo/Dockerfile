# Imagem mínima para servir a web demo ESTÁTICA.
# Sem Node, sem framework, sem backend. Apenas arquivos estáticos via Nginx.
FROM nginx:alpine

# Copia o conteúdo da web demo para o diretório padrão servido pelo Nginx.
# O .dockerignore evita baquear o próprio Dockerfile e vídeos pesados na imagem.
COPY . /usr/share/nginx/html

EXPOSE 80
