тк задание совсем маленькео, не стал делить на библеотеки. 
Если бы приложение было больше, то разделение бы шло UrlShortener.Data, UrlShortener.Application, UrlShortener.Host.
Тоже самое с тестами, тк логики особо и нет, тут тесты не стал делать.
Опять же, было бы большое приложение, генерацию qr code спрятал бы за провайдеров, чтобы если вдруг у нас генерация переехала в отлельный сервис, то код не менялся.
Из задания не совсем понятно, какие ссылки разрешены, а какие нет, поэтому выбрал, что доступны любые валидные ссылки.
