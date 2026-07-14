namespace Hibit.Web.Content;

public static class SiteContent
{
    public const string HeroImageUrl =
        "https://images.pexels.com/photos/6804068/pexels-photo-6804068.jpeg?auto=compress&cs=tinysrgb&w=1600";

    public const string StoryImageUrl =
        "https://images.pexels.com/photos/7988116/pexels-photo-7988116.jpeg?auto=compress&cs=tinysrgb&w=1200";

    public static class Company
    {
        public const string Name = "Hibit";
        public const string Tagline = "Desenvolvimento de software sob medida";
        public const string Description =
            "Somos uma empresa de tecnologia especializada em transformar ideias em produtos digitais robustos, escaláveis e seguros.";
        public const string WhatsApp = "5535920014611";
        public const string WhatsAppDisplay = "(35) 9.2001-4611";
        public const string WhatsAppUrl = "https://wa.me/5535920014611";
        public const string WhatsAppLabel = "Fale conosco pelo WhatsApp";
        public const string Email = "contato@hibit.com.br";
        public const string Address = "Rua Arilton Pinto de Almeida, 127 - Bom Sucesso/MG";
        public const string Phone = "(35) 9.2001-4611";
        public const string BusinessHours = "Segunda a sexta, 9h às 18h";
        public const string MapEmbedUrl =
            "https://www.google.com/maps?q=Rua+Arilton+Pinto+de+Almeida,+127,+Bom+Sucesso,+MG&output=embed";
    }

    public static class Navigation
    {
        public static readonly NavItem[] Items =
        [
            new("Início", "/", true),
            new("Sobre", "/sobre", false),
            new("Contato", "/contato", false)
        ];

        public record NavItem(string Label, string Href, bool Exact);
    }

    public static class Home
    {
        public const string HeroBadge = "Engenharia de software de ponta";
        public const string HeroTitle = "Construímos software que impulsiona o seu negócio";
        public const string HeroSubtitle =
            "Da arquitetura à entrega contínua, a Hibit projeta, desenvolve e evolui soluções digitais com foco em performance, segurança e experiência do usuário.";
        public const string CtaPrimary = "Iniciar um projeto";
        public const string CtaSecondary = "Conheça a Hibit";
        public const string ServicesLabel = "O que fazemos";
        public const string ServicesTitle = "Soluções completas em software";
        public const string ServicesSubtitle =
            "Do planejamento à operação, entregamos tecnologia que gera resultados mensuráveis para o seu negócio.";
        public const string ProcessLabel = "Como trabalhamos";
        public const string ProcessTitle = "Processo transparente e ágil";
        public const string ProcessSubtitle =
            "Cada etapa é planejada para reduzir riscos e garantir entregas de valor desde o primeiro sprint.";
        public const string TechLabel = "Stack tecnológica";
        public const string TechTitle = "Ferramentas que dominamos";
        public const string TechSubtitle =
            "Utilizamos tecnologias maduras e amplamente adotadas no mercado para garantir manutenibilidade e performance.";
        public const string CtaTitle = "Pronto para tirar seu projeto do papel?";
        public const string CtaSubtitle =
            "Conte-nos sobre o desafio. Retornamos com uma proposta clara e sem compromisso.";
        public const string CtaButton = "Solicitar orçamento";

        public static readonly StatItem[] Stats =
        [
            new("+5", "Anos de experiência"),
            new("50+", "Projetos entregues"),
            new("99%", "Satisfação dos clientes"),
            new("24/7", "Suporte dedicado")
        ];

        public static readonly ServiceItem[] Services =
        [
            new("Desenvolvimento sob medida",
                "Aplicações web e APIs construídas com arquitetura limpa, testes automatizados e código sustentável."),
            new("Arquitetura de sistemas",
                "Desenho de soluções escaláveis com microsserviços, integrações e padrões de mercado."),
            new("Modernização de legado",
                "Migração gradual de sistemas antigos para stacks modernas sem interromper a operação."),
            new("Cloud & DevOps",
                "Pipelines CI/CD, infraestrutura como código e deploy automatizado em ambientes cloud."),
            new("Consultoria técnica",
                "Auditoria de código, revisão de arquitetura e mentoria para equipes de desenvolvimento."),
            new("Manutenção evolutiva",
                "Evolução contínua do produto com monitoramento, correções e novas funcionalidades.")
        ];

        public static readonly ProcessStep[] Process =
        [
            new("01", "Descoberta",
                "Entendemos o problema, mapeamos requisitos e definimos escopo e prioridades."),
            new("02", "Design & Arquitetura",
                "Projetamos a solução técnica, UX e fluxos antes de escrever a primeira linha de código."),
            new("03", "Desenvolvimento",
                "Sprints iterativos com entregas frequentes, testes e revisão de código contínua."),
            new("04", "Entrega & Evolução",
                "Deploy seguro, monitoramento em produção e suporte para evolução do produto.")
        ];

        public static readonly string[] Technologies =
        [
            ".NET", "Angular", "TypeScript", "MySQL", "RabbitMQ", "Docker",
            "Inteligencia Artificial", "Python", "Consultoria"
        ];

        public record StatItem(string Value, string Label);
        public record ServiceItem(string Title, string Description);
        public record ProcessStep(string Step, string Title, string Description);
    }

    public static class About
    {
        public const string PageTitle = "Sobre a Hibit";
        public const string PageSubtitle =
            "Uma equipe apaixonada por engenharia de software, comprometida em entregar valor real para cada cliente.";
        public const string History =
            "A Hibit nasceu da evolução de uma operação de prestação de serviços criada inicialmente para o Vladimir atuar como PJ e atender projetos sob demanda. Com o aumento da confiança dos clientes e da complexidade das entregas, essa base foi estruturada, ganhou processos, padrão técnico e posicionamento de produto. Hoje, a Hibit existe como empresa de tecnologia consolidada, focada em engenharia de software, relacionamento de longo prazo e soluções que acompanham o crescimento de cada cliente.";
        public const string Mission =
            "Desenvolver soluções digitais confiáveis que resolvam problemas reais e gerem impacto mensurável para nossos clientes.";
        public const string Vision =
            "Ser referência em desenvolvimento de software no Brasil, reconhecida pela qualidade técnica e pela relação de confiança com cada parceiro.";
        public const string Culture =
            "Acreditamos em times multidisciplinares, aprendizado contínuo e decisões baseadas em dados. Nossa cultura valoriza a colaboração, o respeito e a busca incessante por qualidade.";
        public const string CtaTitle = "Vamos construir algo incrível juntos?";
        public const string CtaSubtitle = "Conte sobre seu projeto e descubra como podemos ajudar.";
        public const string CtaButton = "Entrar em contato";

        public static readonly TimelineItem[] Timeline =
        [
            new("Origem", "Atuação como prestação de serviços PJ para projetos personalizados."),
            new("Transição", "Padronização técnica, expansão do time e foco em processos escaláveis."),
            new("Hibit hoje", "Empresa de software com entregas contínuas e parceria de longo prazo.")
        ];

        public static readonly ValueItem[] Values =
        [
            new("Excelência técnica", "Código limpo, arquitetura sólida e boas práticas em cada entrega."),
            new("Transparência", "Comunicação clara sobre prazos, riscos e progresso em todas as etapas."),
            new("Compromisso", "Tratamos cada projeto como se fosse nosso, com dedicação e responsabilidade."),
            new("Inovação pragmática", "Tecnologias modernas aplicadas com critério, sempre a serviço do negócio.")
        ];

        public static readonly string[] Differentiators =
        [
            "Equipe sênior com experiência em projetos de alta complexidade",
            "Metodologia ágil com entregas incrementais e feedback contínuo",
            "Segurança e conformidade (LGPD) integradas desde o design",
            "Documentação técnica e transferência de conhecimento",
            "Suporte pós-entrega e evolução contínua do produto"
        ];

        public record TimelineItem(string Title, string Description);
        public record ValueItem(string Title, string Description);
    }

    public static class Contact
    {
        public const string Title = "Vamos conversar";
        public const string Subtitle =
            "Preencha o formulário ou entre em contato pelos canais abaixo. Respondemos em até 1 dia útil.";
        public const string FormTitle = "Envie sua mensagem";
        public const string FormNameLabel = "Nome *";
        public const string FormEmailLabel = "E-mail *";
        public const string FormPhoneLabel = "Telefone";
        public const string FormSubjectLabel = "Assunto *";
        public const string FormMessageLabel = "Mensagem *";
        public const string FormConsentLabel = "Li e concordo com a Política de Privacidade *";
        public const string FormSubmitButton = "Enviar mensagem";
        public const string FormSubmitting = "Enviando...";
        public const string SuccessMessage = "Mensagem enviada com sucesso! Entraremos em contato em breve.";
        public const string ErrorMessage =
            "Não foi possível enviar sua mensagem. Verifique se a API está rodando e tente novamente.";
        public const string InfoTitle = "Informações";
        public const string MapTitle = "Mapa da Hibit";
    }

    public static class Privacy
    {
        public const string Title = "Política de Privacidade";
        public const string Summary =
            "A Hibit respeita sua privacidade. Os dados informados no formulário de contato são utilizados exclusivamente para responder à sua solicitação, em conformidade com a LGPD.";

        public static readonly SectionItem[] Sections =
        [
            new("Coleta de dados",
                "Coletamos apenas os dados informados voluntariamente no formulário de contato: nome, e-mail, telefone (opcional), assunto e mensagem."),
            new("Finalidade",
                "Os dados são utilizados exclusivamente para responder à sua solicitação de contato, em conformidade com a Lei Geral de Proteção de Dados (LGPD)."),
            new("Armazenamento",
                "As mensagens enviadas por este site não são armazenadas em banco de dados da aplicação web. O tratamento posterior é realizado de forma segura por sistemas internos da Hibit."),
            new("Seus direitos",
                "Você pode solicitar informações sobre o tratamento dos seus dados entrando em contato pelo e-mail contato@hibit.com.br ou pela nossa página de contato.")
        ];

        public record SectionItem(string Title, string Content);
    }

    public static class Footer
    {
        public static readonly LinkItem[] QuickLinks =
        [
            new("Início", "/"),
            new("Sobre", "/sobre"),
            new("Contato", "/contato"),
            new("Privacidade", "/privacidade")
        ];

        public record LinkItem(string Text, string Href);
    }
}
