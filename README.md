# zmqp-ping
Это простая утилита наподобии `ping` созданная в качестве практики компьютерных сетей, представляет собой простую упаковку и отправку ICMP пакета по адресу;

## Как использовать ? (а зачем?)
Приложение принимает 2 аргумента, где обязательным является первый, который является адресом. Второй аргумент представляет собой количество попыток отправки ICMP пакета (по умолчанию = 4)

Следовательно, для запуска необходимо ввести команду вида:
```
dotnet run google.com 
```

Выходной ответ:
```
destination address: google.com; destination host: 108.177.14.102, bytes: 64, result: 64; time: 13,1996 ms; time-to-live: 64; try: 1
destination address: google.com; destination host: 108.177.14.113, bytes: 64, result: 64; time: 2,8792 ms; time-to-live: 64; try: 2
destination address: google.com; destination host: 108.177.14.113, bytes: 64, result: 64; time: 0,9235 ms; time-to-live: 64; try: 3
destination address: google.com; destination host: 108.177.14.138, bytes: 64, result: 64; time: 1,7389 ms; time-to-live: 64; try: 4
4 packets transmitted, 4 packets received, 0% loss
```