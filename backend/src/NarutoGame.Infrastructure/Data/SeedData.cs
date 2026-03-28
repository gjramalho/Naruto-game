using NarutoGame.Core.Entities;

namespace NarutoGame.Infrastructure.Data;

public static class SeedData
{
    public static List<Npc> GetDefaultNpcs()
    {
        return new List<Npc>
        {
            new Npc
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Key = "naruto",
                Name = "Naruto Uzumaki",
                Avatar = "🍜",
                Color = "bg-orange-500",
                NpcType = "ally",
                Dialogues = new List<NpcDialogue>
                {
                    new() { Id = Guid.NewGuid(), NpcId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Text = "Olá! Bem-vindo à Vila da Folha, dattebayo!", Order = 1 },
                    new() { Id = Guid.NewGuid(), NpcId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Text = "Vou me tornar o melhor Hokage de todos os tempos!", Order = 2 },
                    new() { Id = Guid.NewGuid(), NpcId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Text = "Nunca desista dos seus sonhos, esse é o meu jeito ninja!", Order = 3 },
                    new() { Id = Guid.NewGuid(), NpcId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Text = "Quer treinar comigo? Posso te ensinar o Rasengan!", Order = 4 }
                }
            },
            new Npc
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Key = "sasuke",
                Name = "Sasuke Uchiha",
                Avatar = "👁️",
                Color = "bg-red-600",
                NpcType = "ally",
                Dialogues = new List<NpcDialogue>
                {
                    new() { Id = Guid.NewGuid(), NpcId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Text = "Hmph... Outro ninja fraco.", Order = 1 },
                    new() { Id = Guid.NewGuid(), NpcId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Text = "O poder é tudo. Sem ele, você não pode proteger nada.", Order = 2 },
                    new() { Id = Guid.NewGuid(), NpcId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Text = "Meu Sharingan pode ver através de qualquer técnica.", Order = 3 },
                    new() { Id = Guid.NewGuid(), NpcId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Text = "Treine mais se quiser chegar ao meu nível.", Order = 4 }
                }
            },
            new Npc
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Key = "sakura",
                Name = "Sakura Haruno",
                Avatar = "🌸",
                Color = "bg-pink-500",
                NpcType = "ally",
                Dialogues = new List<NpcDialogue>
                {
                    new() { Id = Guid.NewGuid(), NpcId = Guid.Parse("33333333-3333-3333-3333-333333333333"), Text = "Olá! Precisa de cura? Sou uma ninja médica.", Order = 1 },
                    new() { Id = Guid.NewGuid(), NpcId = Guid.Parse("33333333-3333-3333-3333-333333333333"), Text = "A força não vem apenas dos músculos, mas também da mente.", Order = 2 },
                    new() { Id = Guid.NewGuid(), NpcId = Guid.Parse("33333333-3333-3333-3333-333333333333"), Text = "Posso te ensinar algumas técnicas médicas básicas.", Order = 3 },
                    new() { Id = Guid.NewGuid(), NpcId = Guid.Parse("33333333-3333-3333-3333-333333333333"), Text = "Cuidado para não se machucar nos treinos!", Order = 4 }
                }
            },
            new Npc
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Key = "kakashi",
                Name = "Kakashi Hatake",
                Avatar = "⚡",
                Color = "bg-gray-600",
                NpcType = "ally",
                Dialogues = new List<NpcDialogue>
                {
                    new() { Id = Guid.NewGuid(), NpcId = Guid.Parse("44444444-4444-4444-4444-444444444444"), Text = "Yo! Desculpe o atraso, me perdi no caminho da vida.", Order = 1 },
                    new() { Id = Guid.NewGuid(), NpcId = Guid.Parse("44444444-4444-4444-4444-444444444444"), Text = "Um ninja deve ver através da decepção.", Order = 2 },
                    new() { Id = Guid.NewGuid(), NpcId = Guid.Parse("44444444-4444-4444-4444-444444444444"), Text = "Trabalho em equipe é essencial para um ninja.", Order = 3 },
                    new() { Id = Guid.NewGuid(), NpcId = Guid.Parse("44444444-4444-4444-4444-444444444444"), Text = "Que tal lermos um pouco do Icha Icha Paradise?", Order = 4 }
                }
            }
        };
    }

    public static List<Mission> GetDefaultMissions()
    {
        return new List<Mission>
        {
            new Mission
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Title = "Primeiro Treino",
                Description = "Treine com Naruto para aprender o básico dos ninjutsus.",
                XpReward = 50,
                Difficulty = 1,
                IsDaily = true,
                IsActive = true
            },
            new Mission
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Title = "Coleta de Ervas",
                Description = "Colete 10 ervas medicinais na floresta próxima para Sakura.",
                XpReward = 30,
                Difficulty = 1,
                IsDaily = true,
                IsActive = true
            },
            new Mission
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                Title = "Defesa do Portão",
                Description = "Ajude a defender o portão principal da Vila da Folha.",
                XpReward = 100,
                Difficulty = 2,
                IsDaily = false,
                IsActive = true
            },
            new Mission
            {
                Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                Title = "Treino de Taijutsu",
                Description = "Participe de um treino intenso de taijutsu com Rock Lee.",
                XpReward = 75,
                Difficulty = 2,
                IsDaily = true,
                IsActive = true
            },
            new Mission
            {
                Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Title = "Missão de Rank D",
                Description = "Acompanhe um ninja sênior em uma missão de rank D.",
                XpReward = 150,
                Difficulty = 3,
                IsDaily = false,
                IsActive = true
            }
        };
    }
}
