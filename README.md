# Oportunidade
Estamos contratando um desenvolvedor para atuar diretamente com o time de arquitetura da [ConectCar](http://conectcar.com). Se tiver interesse de participar, faça o exercício de programção abaixo.

Farão parte de suas responsabilidades:

* Implementar os padrões de arquitetura
* Manter as _libraries_ de desenvolvimento
* Implementar provas de conceito
* Avaliar a solução proposta pelo time de desenvolvimento
* Apoiar no desenho de soluções de TI
* Coaching de arquitetura/padrões/referências/tecnologias para o time de desenvolvimento
* Praticar automatização de testes
* Praticar automatização de infraestrutura 

# Exercício de programação
Você deverá fazer um _fork_ desse repositório e nos enviar um _pull request_ com a solução em que três microsserviços se comuniquem de modo que atendam os requisitos de escalabilidade.

O código atual é um mero _entrypoint_ para a solução e você pode implementá-la do jeito que achar mais performático e que atendam os requisitos. Porém é importante que cada projeto rode em sua própria instância para permitir a escala individualmente.

## Cenário
**_[Esse é um cenário hipotético com a finalidade de testar as habilidades do candidato]_**

Quando um cliente adquire um plano da ConectCar, nós precisamos solicitar a um fornecedor terceirizado para que envie nosso adesivo ao cliente.

Além disso, ao adquirir o plano, o custo do adesivo é cobrado do cliente. Essa cobrança é realizada através de um _gateway_ de pagamento.

Quando tanto a cobrança quanto o aviso de postagem é recebido nós avisamos o cliente que o adesivo está a caminho.

A ConectCar quer implantar esse cenário usando uma arquitetura de _microservices_ que pode ser facilmente escalada, visto que os microsserviços que comunicam com os parceiros podem sofrer com a latência e falhas de comunicação.

## Solução

O código atual apresenta três aplicações console em .NET Core:
* Pedidos
* EnvioAdesivos
* Cobranca

### Pedidos

O objetivo desse microsserviço é orquestrar os passos que um pedido precisa realizar para ser concluído. Desta forma, um pedido é recebido e transmitido para os demais microsserviços. Uma vez que cada microsserviço realizou sua tarefa, a informação de pedido concluído é exibida.

Essa aplicação receberia cada uma das requisições dos clientes, mas, para efeitos deste teste, é realizada a leitura de um arquivo com os pedidos (já implementado). Ao ler um pedido, antes de postar para os demais serviços, o método `ProcessandoPedido` da classe `Logger` deverá ser chamado. 

Cada um dos pedidos é então enviado a cada microsserviço (EnvioAdesivos e Cobranca).

Ao receber as respostas de envio e de cobrança, o método `PedidoProcessado` da classe `Logger` deverá ser chamado.

### EnvioAdesivos

Essa aplicação recebe um pedido e o processa, simulando a preparação para postagem do adesivo. 

Existe uma simulação de integração com o serviço do fornecedor que pode ser chamado em `Integracao.EnviarAdesivo()`. Essa simulação tem um atraso de 200ms e pode falhar em 10% dos casos, causando uma exception.

Em caso de falha ao tentar enviar um adesivo, uma retentativa deverá ser realizada novamente em 5s. Não existe limite para tentativas de envio.

Ao conseguir realizar o envio do adesivo, um evento de domínio deve ser gerado para que a aplicação de pedidos saiba desse envio.

### Cobranca

O objetivo desse microsserviço é realizar a cobrança simulando a integração com um _gateway_ de pagamento.

Existe uma simulação de integração com o serviço do _gateway_ que pode ser chamado em `Integracao.Cobrar()`. Essa simulação tem um atraso de 500ms e pode falhar em 10% dos casos, causando uma exception.

Em caso de falha ao tentar cobrar, uma retentativa deverá ser realizada novamente em 5s. Não existe limite para tentativas de cobrança.

Ao conseguir realizar a cobrança, um evento de domínio deve ser disparado para que a aplicação de pedidos saiba do sucesso dessa cobrança.

## Características das aplicações

* As aplicações de envio e de cobrança precisam ser escaláveis.
* Um pedido não pode ser enviado ou cobrado mais de uma vez por motivos óbvios ;)
* Não se esqueça dos testes automatizados
* As classes das aplicações deste repositório podem ser alteradas a vontade. Só não altere a simulação de atraso e falhas das classes `Integracao`

## Opcionais que fazem a diferença

* Lembre-se que o _deploy_ de microsserviços pode ser complicado. Pensar em uma solução para facilitar esse processo é um excelente diferencial.

* Alguns eventos podem ser úteis não apenas para a aplicação de pedidos. Por uma questão de melhoria em nosso produto, poderíamos, por exemplo, querer notificar nossos clientes quando seu adesivo é enviado. Ou então, integrar um serviço de rastreio deste envio. Como esse evento poderia ser reutilizado no futuro por outras aplicações como essas de notificação ou rastreio?