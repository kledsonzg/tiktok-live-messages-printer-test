# tiktok-live-messages-printer-test
Este programa foi desenvolvido apenas por curiosidade minha.
### O que foi utilizado?
- C# .NET
-> O Programa cria uma instância da classe HttpListener para "capturar" as requisições feitas no Power Automate e printar o 'corpo' obtido da requisição.
- Microsoft Power Automate
->  Um código javascript é injetado utilizando o Power Automate para fazer a leitura do chat com o código HTML do TikTok.
->  Após fazer a leitura, é feito uma requisição HTTP POST para o servidor.

### Observações:
Por estar utilizando o Power Automate para obter o chat das lives, é necessário ter uma instância de navegador aberta (Foi utilizado O Microsoft Edge nos testes).
