using System;
using System.Collections.Generic;
using System.Linq;

class JokempoMenosUm
{
    // Ranking: chave = nome do jogador, valor = (vitórias, empates, derrotas)
    static Dictionary<string, (int vitorias, int empates, int derrotas)> ranking = new Dictionary<string, (int, int, int)>();
    static Random random = new Random();

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        bool sair = false;

        while (!sair)
        {
            Console.Clear();
            Console.WriteLine("😀 Bem-vindo ao Jokempo Menos Um!");
            Console.WriteLine("1 - Jogar");
            Console.WriteLine("2 - Exibir Ranking");
            Console.WriteLine("0 - Sair");
            string opcaoMenu = GetValidOption(new List<string> { "0", "1", "2" });

            switch (opcaoMenu)
            {
                case "1":
                    Jogar();
                    break;
                case "2":
                    ExibirRanking();
                    Console.WriteLine("\nDigite 1 para voltar ao menu principal ou 0 para sair:");
                    string volta = GetValidOption(new List<string> { "0", "1" });
                    if (volta == "0")
                        sair = true;
                    break;
                case "0":
                    sair = true;
                    break;
            }
        }

        Console.WriteLine("👋 Tchau! Até a próxima!");
        Console.ReadLine();
    }

    // Método para jogar uma sessão com um jogador
    static void Jogar()
    {
        Console.Clear();
        Console.WriteLine("Qual é o seu nome?");
        string nomeJogador = Console.ReadLine().Trim();
        while (string.IsNullOrEmpty(nomeJogador))
        {
            Console.WriteLine("Nome inválido. Por favor, digite seu nome:");
            nomeJogador = Console.ReadLine().Trim();
        }
        // Registra automaticamente o jogador se não estiver no ranking
        if (!ranking.ContainsKey(nomeJogador))
            ranking[nomeJogador] = (0, 0, 0);

        bool continuarPartida = true;
        while (continuarPartida)
        {
            Console.Clear();
            Console.WriteLine($"Jogador: {nomeJogador}");
            // Jogada para cada mão do jogador
            string maoEsquerda = ObterJogada("mão esquerda");
            string maoDireita = ObterJogada("mão direita");
            Console.WriteLine($"\nVocê escolheu: Mão esquerda: {maoEsquerda}, Mão direita: {maoDireita}");

            // Jogada do computador 
            string jogadaPC = ObterJogadaComputador();
            Console.WriteLine($"O PC escolheu: {jogadaPC}");

            // O jogador decide qual mão usar para a comparação final
            string maoEscolhida = ObterMaoEscolhida();
            string jogadaJogador = maoEscolhida == "esquerda" ? maoEsquerda : maoDireita;
            
            string jogadaPCFinal = jogadaPC.Split('-')[0];

            // Avalia a jogada (Pedra vence Tesoura, Tesoura vence Papel, Papel vence Pedra)
            bool vitoria = AvaliarJogada(jogadaJogador, jogadaPCFinal);
            AtualizarRanking(nomeJogador, vitoria, jogadaJogador, jogadaPCFinal);

            Console.WriteLine("\nResultado do round:");
            if (jogadaJogador == jogadaPCFinal)
                Console.WriteLine("Empate!");
            else if (vitoria)
                Console.WriteLine("Você venceu!");
            else
                Console.WriteLine("O PC venceu!");

            Console.WriteLine("\nDeseja jogar outra partida com este jogador? (1 - Sim, 0 - Não)");
            string op = GetValidOption(new List<string> { "0", "1" });
            if (op == "0")
                continuarPartida = false;
        }
    }

    // Captura e valida uma opção dentre as opções válidas
    static string GetValidOption(List<string> validOptions)
    {
        string input = Console.ReadLine().Trim();
        while (!validOptions.Contains(input))
        {
            Console.WriteLine("Opção inválida. Por favor, tente novamente:");
            input = Console.ReadLine().Trim();
        }
        return input;
    }

    // Solicita a jogada para uma determinada mão (ex: "mão esquerda" ou "mão direita")
    static string ObterJogada(string descricao)
    {
        Console.WriteLine($"Escolha sua jogada para a {descricao}: Pedra, Papel ou Tesoura");
        string move = Console.ReadLine().Trim();
        List<string> validMoves = new List<string> { "Pedra", "Papel", "Tesoura" };
        while (!validMoves.Contains(move))
        {
            Console.WriteLine("Jogada inválida. Escolha entre Pedra, Papel ou Tesoura:");
            move = Console.ReadLine().Trim();
        }
        return move;
    }

    // Retorna uma jogada aleatória para cada mão do PC no formato "move1-move2"
    static string ObterJogadaComputador()
    {
        string[] moves = { "Pedra", "Papel", "Tesoura" };
        string left = moves[random.Next(moves.Length)];
        string right = moves[random.Next(moves.Length)];
        return left + "-" + right;
    }

    // Solicita qual mão o jogador deseja usar para a comparação final
    static string ObterMaoEscolhida()
    {
        Console.WriteLine("\nEscolha qual mão deseja usar para a comparação final (esquerda ou direita):");
        string escolha = Console.ReadLine().Trim().ToLower();
        while (escolha != "esquerda" && escolha != "direita")
        {
            Console.WriteLine("Opção inválida. Digite 'esquerda' ou 'direita':");
            escolha = Console.ReadLine().Trim().ToLower();
        }
        return escolha;
    }

    // Avalia se a jogada do jogador vence a do PC, de acordo com as regras
    static bool AvaliarJogada(string jogador, string pc)
    {
        if (jogador == pc)
            return false; // empate
        if (jogador == "Pedra" && pc == "Tesoura") return true;
        if (jogador == "Tesoura" && pc == "Papel") return true;
        if (jogador == "Papel" && pc == "Pedra") return true;
        return false;
    }

    // Atualiza o ranking do jogador de forma segura
    static void AtualizarRanking(string nome, bool vitoria, string jogada, string pc)
    {
        var stats = ranking[nome];
        if (jogada == pc)
            stats.empates++;
        else if (vitoria)
            stats.vitorias++;
        else
            stats.derrotas++;
        ranking[nome] = stats;
    }

    // Exibe o ranking ordenado por número de vitórias
    static void ExibirRanking()
    {
        Console.Clear();
        Console.WriteLine("🏆 Ranking de Jogadores:");
        if (ranking.Count == 0)
        {
            Console.WriteLine("Nenhum jogador cadastrado.");
        }
        else
        {
            foreach (var kv in ranking.OrderByDescending(x => x.Value.vitorias))
            {
                Console.WriteLine($"{kv.Key}: {kv.Value.vitorias} vitórias, {kv.Value.empates} empates, {kv.Value.derrotas} derrotas");
            }
        }
        Console.WriteLine("\nPressione Enter para continuar...");
        Console.ReadLine();
    }
}
