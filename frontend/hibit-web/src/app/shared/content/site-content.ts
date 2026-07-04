export interface ServiceItem {
  title: string;
  description: string;
}

export interface StatItem {
  value: string;
  label: string;
}

export interface ProcessStep {
  step: string;
  title: string;
  description: string;
}

export interface SiteContent {
  company: {
    name: string;
    tagline: string;
    description: string;
    email: string;
    whatsapp: {
      phone: string;
      url: string;
      label: string;
    };
  };
  home: {
    heroBadge: string;
    heroTitle: string;
    heroSubtitle: string;
    stats: StatItem[];
    services: ServiceItem[];
    process: ProcessStep[];
    technologies: string[];
    ctaTitle: string;
    ctaSubtitle: string;
  };
  about: {
    pageTitle: string;
    pageSubtitle: string;
    history: string;
    mission: string;
    vision: string;
    values: Array<{ title: string; description: string }>;
    differentiators: string[];
    culture: string;
  };
  contact: {
    title: string;
    subtitle: string;
    address: string;
    phone: string;
    businessHours: string;
  };
  privacy: {
    summary: string;
  };
}

export const siteContent: SiteContent = {
  company: {
    name: 'Hibit',
    tagline: 'Desenvolvimento de software sob medida',
    description:
      'Somos uma empresa de tecnologia especializada em transformar ideias em produtos digitais robustos, escaláveis e seguros.',
    email: 'contato@hibit.com.br',
    whatsapp: {
      phone: '5535920014611',
      url: 'https://wa.me/5535920014611',
      label: 'Fale conosco pelo WhatsApp',
    },
  },
  home: {
    heroBadge: 'Engenharia de software de ponta',
    heroTitle: 'Construímos software que impulsiona o seu negócio',
    heroSubtitle:
      'Da arquitetura à entrega contínua, a Hibit projeta, desenvolve e evolui soluções digitais com foco em performance, segurança e experiência do usuário.',
    stats: [
      { value: '+5', label: 'Anos de experiência' },
      { value: '50+', label: 'Projetos entregues' },
      { value: '99%', label: 'Satisfação dos clientes' },
      { value: '24/7', label: 'Suporte dedicado' },
    ],
    services: [
      {
        title: 'Desenvolvimento sob medida',
        description:
          'Aplicações web e APIs construídas com arquitetura limpa, testes automatizados e código sustentável.',
      },
      {
        title: 'Arquitetura de sistemas',
        description:
          'Desenho de soluções escaláveis com microsserviços, integrações e padrões de mercado.',
      },
      {
        title: 'Modernização de legado',
        description:
          'Migração gradual de sistemas antigos para stacks modernas sem interromper a operação.',
      },
      {
        title: 'Cloud & DevOps',
        description:
          'Pipelines CI/CD, infraestrutura como código e deploy automatizado em ambientes cloud.',
      },
      {
        title: 'Consultoria técnica',
        description:
          'Auditoria de código, revisão de arquitetura e mentoria para equipes de desenvolvimento.',
      },
      {
        title: 'Manutenção evolutiva',
        description:
          'Evolução contínua do produto com monitoramento, correções e novas funcionalidades.',
      },
    ],
    process: [
      {
        step: '01',
        title: 'Descoberta',
        description: 'Entendemos o problema, mapeamos requisitos e definimos escopo e prioridades.',
      },
      {
        step: '02',
        title: 'Design & Arquitetura',
        description: 'Projetamos a solução técnica, UX e fluxos antes de escrever a primeira linha de código.',
      },
      {
        step: '03',
        title: 'Desenvolvimento',
        description: 'Sprints iterativos com entregas frequentes, testes e revisão de código contínua.',
      },
      {
        step: '04',
        title: 'Entrega & Evolução',
        description: 'Deploy seguro, monitoramento em produção e suporte para evolução do produto.',
      },
    ],
    technologies: [
      '.NET',
      'Angular',
      'TypeScript',
      'MySQL',
      'RabbitMQ',
      'Docker',
      'Inteligencia Artificial',
      'Python',
      'Consultoria',
    ],
    ctaTitle: 'Pronto para tirar seu projeto do papel?',
    ctaSubtitle: 'Conte-nos sobre o desafio. Retornamos com uma proposta clara e sem compromisso.',
  },
  about: {
    pageTitle: 'Sobre a Hibit',
    pageSubtitle:
      'Uma equipe apaixonada por engenharia de software, comprometida em entregar valor real para cada cliente.',
    history:
      'A Hibit nasceu da evolução de uma operação de prestação de serviços criada inicialmente para o Vladimir atuar como PJ e atender projetos sob demanda. Com o aumento da confiança dos clientes e da complexidade das entregas, essa base foi estruturada, ganhou processos, padrão técnico e posicionamento de produto. Hoje, a Hibit existe como empresa de tecnologia consolidada, focada em engenharia de software, relacionamento de longo prazo e soluções que acompanham o crescimento de cada cliente.',
    mission:
      'Desenvolver soluções digitais confiáveis que resolvam problemas reais e gerem impacto mensurável para nossos clientes.',
    vision:
      'Ser referência em desenvolvimento de software no Brasil, reconhecida pela qualidade técnica e pela relação de confiança com cada parceiro.',
    values: [
      {
        title: 'Excelência técnica',
        description: 'Código limpo, arquitetura sólida e boas práticas em cada entrega.',
      },
      {
        title: 'Transparência',
        description: 'Comunicação clara sobre prazos, riscos e progresso em todas as etapas.',
      },
      {
        title: 'Compromisso',
        description: 'Tratamos cada projeto como se fosse nosso, com dedicação e responsabilidade.',
      },
      {
        title: 'Inovação pragmática',
        description: 'Tecnologias modernas aplicadas com critério, sempre a serviço do negócio.',
      },
    ],
    differentiators: [
      'Equipe sênior com experiência em projetos de alta complexidade',
      'Metodologia ágil com entregas incrementais e feedback contínuo',
      'Segurança e conformidade (LGPD) integradas desde o design',
      'Documentação técnica e transferência de conhecimento',
      'Suporte pós-entrega e evolução contínua do produto',
    ],
    culture:
      'Acreditamos em times multidisciplinares, aprendizado contínuo e decisões baseadas em dados. Nossa cultura valoriza a colaboração, o respeito e a busca incessante por qualidade.',
  },
  contact: {
    title: 'Vamos conversar',
    subtitle:
      'Preencha o formulário ou entre em contato pelos canais abaixo. Respondemos em até 1 dia útil.',
    address: 'Rua Arilton Pinto de Almeida, 127 - Bom Sucesso/MG',
    phone: '(35) 9.2001-4611',
    businessHours: 'Segunda a sexta, 9h às 18h',
  },
  privacy: {
    summary:
      'A Hibit respeita sua privacidade. Os dados informados no formulário de contato são utilizados exclusivamente para responder à sua solicitação, em conformidade com a LGPD.',
  },
};
