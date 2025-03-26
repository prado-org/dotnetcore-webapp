# GitHub Copilot Demo

Nesse documento está descrito os passos necessários para a demo de GH Copilot

## Copilot PR Review

O código abaixo deve ser inserido dentro de um controller. Esse códio é **ruim** de proposito. Quando criar o PR o Copilot PR Review vai validar esse código e sugerir um melhor.

```cs
private void CreateXml(string itemName, bool isComplete)
{
    _logger.LogInformation("Método CreateXml");
    using (XmlWriter writer = XmlWriter.Create("todos.xml"))
    {
        writer.WriteStartDocument();
        writer.WriteRaw("<todo><name>" + itemName + "</name><isComplete>" + isComplete.ToString() + "</isComplete></todo>");
        writer.WriteEndElement();
        writer.WriteEndDocument();
    }
}
```
## Prompts

### Copilot Chat

@azure /changeTenant

@azure liste todos os resources groups

@azure liste os recursos dentro do resource group rg-dotnetproject-dev

@azure qual o custo mensal do resrouce group rg-dotnetproject-dev

@azure qual a subscriptionID onde esta o resource sgroup rg-dotnetproject-dev

@azure qual o custo mensal do resrouce group rg-dotnetproject-dev que esta na subscription e84af34a-bbd2-4d60-8776-829485d3e735

@github liste os prs que estao abertos

@github descreva os detalhes do pr Pr - Feature/apresentacao 11 12

@workspace descreva qual o objeitvo desse repositorio. Liste todos os projetos contidos. Para cada projeto liste a tecnologia, framework e versão utilizada.

@workspace descreva as camadas do projeto MyFirstProject.TodoApi. Identifique quais são as camadas de API, Dominio, Infraestrutura, Data. Liste o relacionamento e dependencias entre essas camadas.

[Api - TodoItemController.cs]
crie um método para salvar um TodoItem

crie um método para editar um todo item

### Copilot Edit

[Api - TodoItemController.cs]
implemente uma regra de negocio para os metodos de criar e editar para verificar se o campo Name possui caracteres especiais. Se conter retornar uma mensagem de erro "Campo Name é inválido"

adicione em todos os métodos, um registro de log que o método foi chamado. Se o método ja tiver esse registro de log, ignore.

Em todos os métodos que contém tratamento de exceção, adicione um registro no log como Error.

atue como um engenheiro de software senior, com amplo conhecimento em dotnet core. Sua tarefa é revisar esse controller analisando seu comportamento do ponto de vista de segurança, performance e eficiência e a utilização de métodos assincronos

### Copilot Vision (Imagem)
Adicione a imagem (images/todo-app-classic.png) no chat e faça os seguintes prompts

descreva os componentes dessa arquitetura, e suas relações

implemente a camada de application utilizando o framework dotnet core 8 e a linguagem C#
