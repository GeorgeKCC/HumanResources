#!/bin/bash
# Script: set_kibana_password.sh
echo "Esperando a que el clúster esté completamente operativo..."

# Comando para cambiar la contraseña del usuario kibana_system
# La variable de entorno ELASTIC_PASSWORD se usa para autenticar la solicitud.
# El -X POST -H "Content-Type: application/json" envía los datos
# El -d envía el JSON para cambiar la contraseña
# El -u usa el usuario 'elastic' y su contraseña.

curl -s -X POST "http://elasticsearch:9200/_security/user/kibana_system/_password?pretty" \
  -H 'Content-Type: application/json' \
  -u ${ELASTIC_USERNAME}:${ELASTIC_PASSWORD} \
  -d '{
    "password" : "'${KIBANA_SYSTEM_PASSWORD}'"
  }'

if [ $? -eq 0 ]; then
  echo "Contraseña para kibana_system establecida con éxito."
else
  echo "Error al establecer la contraseña para kibana_system."
  exit 1
fi