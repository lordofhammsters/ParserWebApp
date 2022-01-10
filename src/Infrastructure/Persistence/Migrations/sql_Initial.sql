CREATE TABLE [StatisticSite] (
    [Id] int NOT NULL IDENTITY,
    [Url] nvarchar(max) NOT NULL,
    [Created] datetime NOT NULL,
    CONSTRAINT [PK_StatisticSite] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [StatisticSiteWord] (
    [Id] int NOT NULL IDENTITY,
    [StatisticSiteId] int NOT NULL,
    [Word] nvarchar(max) NOT NULL,
    [Count] int NOT NULL,
    CONSTRAINT [PK_StatisticSiteWord] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_StatisticSiteWord_StatisticSite_StatisticSiteId] FOREIGN KEY ([StatisticSiteId]) REFERENCES [StatisticSite] ([Id]) ON DELETE CASCADE
);
GO


CREATE INDEX [IX_StatisticSiteWord_StatisticSiteId] ON [StatisticSiteWord] ([StatisticSiteId]);
GO


