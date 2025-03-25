# GitHub Copilot Demo

Nesse documento está descrito os passos necessários para a demo de GH Copilot

## Copilot PR Review

O código abaixo deve ser inserido dentro de um controller. Esse códio é ruim de proposito. Quando criar o PR o Copilot PR Review vai validar esse código e sugerir um melhor.

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

@azure /changeTenant

@azure liste todos os resources groups

@azure liste os recursos dentro do resource group rg-dotnetproject-dev

@azure qual o custo mensal do resrouce group rg-dotnetproject-dev

@azure qual a subscriptionID onde esta o resource sgroup rg-dotnetproject-dev

@azure qual o custo mensal do resrouce group rg-dotnetproject-dev que esta na subscription e84af34a-bbd2-4d60-8776-829485d3e735

@github liste os prs que estao abertos

@github descreva os detalhes do pr Pr - Feature/apresentacao 11 12
