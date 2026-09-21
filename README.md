# SantaCruz.Order

## Detalhes do Projeto

    - Está dividido em Api => Application => Domain => Infrastructure seguindo os preceitos do Clean Code (SOLID) e Arquitetura Hexagonal
    - Domain é a camada que define as Ports, e não depende de nenhuma outra camada
    - Infrastructure é a camada que define os Adapters externos (comunicação com domínios fora da aplicação)
    - Application é a camada da aplicação propriamente dita e contém os Adapters de domínios internos da aplicação
    - Houve um esforço para manter boas práticas de aplicações DDD, como uma escrita pautada em negócios/casos de usos, bem como evitar redundâncias de nomes
 
## Detalhes da implementação do Worker

    - Para construir a chamada de processamento dos pedidos foi criado um BackgroundWorker
    - As propriedades dele podem ser controladas pelo appsettings.json do projeto SantaCruz.Processing
        * ServiceOk: Quando 'false' simula que a integração externa está indisponível, alterando o fluxo de processamento para as retentativas
        * MaxAttempts : Define o máximo de retentativas que a aplicação fará em um pedido que permanece na fila.
    - Para construir o fluxo do Worker foi aplicado um padrão de ChainOfResponsibility, em que cada Passo da Cadeia de Responsabilidades é implementado por 
    um UseCase específico, responsável por uma única tarefa e por apontar qual a próxima tarefa a ser executada.
    - Uma Cadeia específica pode ter sub cadeias que rodam de acordo com a necessidade (como foi feito para implementar a necessidade de cada pedido ser executado em seu ciclo próprio de processamento, mas
    de maneira assíncrona)

## Banco de Dados

    - Os Scripts de Criação das tabelas e população da tabela produto estão em SantaCruz.Application -> Scripts -> PostgreSQL
    - Os Scripts se encontram em ordem numérica
    - Na mesma pasta se encontra um comando shell para executar o docker com as configurações utilizadas
