# NetworkingDemo
Everything was build on Unity’s default shooter project. I used KCP transport
Implemented a general Lobby to host a server and get in game immediately or join a hosted server. 
I synced: 
- player+weapon movement and rotation - client to server, Network Transform sync, non transform variables using [SyncVar]
- player weapons inventory - weapon change command
- player shooting projectile - player command to spawn projectile
- projectile hitting something - visual effect for every player, server handles hit
- player health, death, respawn - server based, sending info to client. respawn position change handled by client
- enemy kill, health, attacks, animation - server to client
- items pickups and fully local health pickups - client based, pickup message on trigger enter. server spawning item drops
- obstacles health and loot drops - server based
- Sync player inventory - request/response system the worker system uses. Whole inventory logged in console after pickup
- Weapons “animation” sync - visual transform movement. Command sync of relevant variables

Video:


https://github.com/user-attachments/assets/f055b198-f23a-4f76-aa7b-4fa2abb63955

