using System;
using System.Collections.Generic;

Dictionary<string, (int vitórias, int empates, int derrotas)> jogadores = new Dictionary<string, (int, int, int)>();

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("😀 Olá! Vamos jogar Jokempo?");
Console.WriteLine("1 - Sim ou 0 - Não");

var continuar = ValidarEntrada('0', '1');

while (continuar != '0')
{
    string nomeJogador = ObterNomeJogador();

    if (!jogadores.ContainsKey(nomeJogador))
    {
        jogadores[nomeJogador] = (0, 0, 0);
    }

    Console.WriteLine($"Bem-vindo, {nomeJogador}! Vamos começar...");

    do
    {
        char opcao = ObterOpcaoJogador();
        var opcaoPC = new Random().Next(3);
        bool vitoria = JogarRodada(opcao, opcaoPC, nomeJogador);
        AtualizarEstatisticas(nomeJogador, vitoria, opcao, opcaoPC);

        Console.WriteLine("\nQuer jogar de novo?");
        Console.WriteLine("1 - Sim, 0 - Não");
    } while (Console.ReadKey().KeyChar == '1');

    Console.WriteLine("\nO que deseja fazer agora?");
    Console.WriteLine("1 - Continuar com outro jogador, 2 - Listar jogadores e estatísticas, 0 - Sair");
    continuar = ValidarEntrada('0', '1', '2');

    if (continuar == '2')
    {
        ListarEstatisticasJogadores(ref continuar);
    }
}

Console.WriteLine("👋 Tchau! Até a próxima");

char ValidarEntrada(params char[] opcoesValidas)
{
    char opcao = Console.ReadKey().KeyChar;
    while (Array.IndexOf(opcoesValidas, opcao) == -1)
    {
        Console.WriteLine("\nOpção inválida. Tente novamente.");
        opcao = Console.ReadKey().KeyChar;
    }
    return opcao;
}

// Métodos auxiliares
string ObterNomeJogador()
{
    Console.WriteLine("\nQual é o seu nome?");
    string nomeJogador = Console.ReadLine();

    while (string.IsNullOrEmpty(nomeJogador))
    {
        Console.WriteLine("Você precisa digitar o seu nome. Pode ser o seu apelido...");
        nomeJogador = Console.ReadLine();
    }

    return nomeJogador;
}

char ObterOpcaoJogador()
{
    Console.WriteLine("Escolha uma opção: 0 - Pedra ✊, 1 - Papel ✋ ou 2 - Tesoura ✌");
    char opcao = Console.ReadKey().KeyChar;
    while (opcao != '0' && opcao != '1' && opcao != '2')
    {
        Console.WriteLine("\nOpção inválida. Escolha 0, 1 ou 2.");
        opcao = Console.ReadKey().KeyChar;
    }
    return opcao;
}

bool JogarRodada(char opcao, int opcaoPC, string nomeJogador)
{
    bool vitoria = false;

    switch (opcao)
    {
        case '0':
            Console.WriteLine("\nVocê escolheu Pedra ✊!");
            vitoria = (opcaoPC == 2);
            break;
        case '1':
            Console.WriteLine("\nVocê escolheu Papel ✋");
            vitoria = (opcaoPC == 0);
            break;
        case '2':
            Console.WriteLine("\nVocê escolheu Tesoura ✌");
            vitoria = (opcaoPC == 1);
            break;
    }

    switch (opcaoPC)
    {
        case 0:
            Console.WriteLine("\nEu escolhi Pedra ✊!");
            break;
        case 1:
            Console.WriteLine("\nEu escolhi Papel ✋");
            break;
        case 2:
            Console.WriteLine("\nEu escolhi Tesoura ✌");
            break;
    }

    if (int.Parse(opcao.ToString()) == opcaoPC)
    {
        Console.WriteLine("\n😀 Legal! Nós empatamos!");
    }
    else if (vitoria)
    {
        Console.WriteLine("\n😀 Parabéns! Você venceu.");
    }
    else
    {
        Console.WriteLine("\n😀 Haha, eu venci! Não foi dessa vez.");
    }

    return vitoria;
}

void AtualizarEstatisticas(string nomeJogador, bool vitoria, char opcao, int opcaoPC)
{
    if (int.Parse(opcao.ToString()) == opcaoPC)
    {
        jogadores[nomeJogador] = (jogadores[nomeJogador].vitórias, jogadores[nomeJogador].empates + 1, jogadores[nomeJogador].derrotas);
    }
    else if (vitoria)
    {
        jogadores[nomeJogador] = (jogadores[nomeJogador].vitórias + 1, jogadores[nomeJogador].empates, jogadores[nomeJogador].derrotas);
    }
    else
    {
        jogadores[nomeJogador] = (jogadores[nomeJogador].vitórias, jogadores[nomeJogador].empates, jogadores[nomeJogador].derrotas + 1);
    }
}

void ListarEstatisticasJogadores(ref char continuar)
{
    Console.WriteLine("\nJogadores e suas estatísticas:\n");
    Console.WriteLine("===================================================================");
    foreach (var jogador in jogadores)
    {
        Console.WriteLine($"{jogador.Key}: {jogador.Value.vitórias} vitórias, {jogador.Value.empates} empates, {jogador.Value.derrotas} derrotas");
    }
    Console.WriteLine("===================================================================\n");

    Console.WriteLine("E agora? Quer iniciar uma nova partida?");
    Console.WriteLine("1 - Sim ou 0 - Não");

    continuar = ValidarEntrada('0', '1');
}