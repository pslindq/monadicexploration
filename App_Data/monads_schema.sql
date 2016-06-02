CREATE TABLE [dbo].[Monads] (
    [MonadID]       INT           IDENTITY (1, 1) NOT NULL,
    [Title]         VARCHAR (150) NOT NULL,
    [URL]           VARCHAR (250) NULL,
    [ShowNodeTypes] BIT           DEFAULT ((1)) NOT NULL,
    [URLSegment]    VARCHAR (100) NOT NULL,
    [AdminPWD]      VARCHAR (500) NOT NULL,
    PRIMARY KEY CLUSTERED ([MonadID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Monads_URLSegment]
    ON [dbo].[Monads]([URLSegment] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Holds the primary collection of top level monads', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Monads';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The standard numeric identifier', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Monads', @level2type = N'COLUMN', @level2name = N'MonadID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The title of the monad for display', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Monads', @level2type = N'COLUMN', @level2name = N'Title';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Optional URL for linking the monad title', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Monads', @level2type = N'COLUMN', @level2name = N'URL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Flag for if the node types switches should be visible', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Monads', @level2type = N'COLUMN', @level2name = N'ShowNodeTypes';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The URL segment this monad is available under', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Monads', @level2type = N'COLUMN', @level2name = N'URLSegment';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The hashed password to access editing this monad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Monads', @level2type = N'COLUMN', @level2name = N'AdminPWD';

CREATE TABLE [dbo].[NodeTypes] (
    [NodeTypeID] INT           IDENTITY (1, 1) NOT NULL,
    [MonadID]    INT           NOT NULL,
    [Sequence]   INT           NOT NULL,
    [Name]       VARCHAR (100) NOT NULL,
    [PluralName] VARCHAR (100) NOT NULL,
    [SlugName]   VARCHAR (50)  NOT NULL,
    [Color]      VARCHAR (6)   NOT NULL,
    PRIMARY KEY CLUSTERED ([NodeTypeID] ASC),
    CONSTRAINT [FK_NodeTypes_Monads] FOREIGN KEY ([MonadID]) REFERENCES [dbo].[Monads] ([MonadID]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_NodeTypes_MonadSequence]
    ON [dbo].[NodeTypes]([MonadID] ASC, [Sequence] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Defines the monad node types', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NodeTypes';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The autokey identifier of the node type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NodeTypes', @level2type = N'COLUMN', @level2name = N'NodeTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reference to the applicable monad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NodeTypes', @level2type = N'COLUMN', @level2name = N'MonadID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The sequence this node type is in relation to it''s siblings', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NodeTypes', @level2type = N'COLUMN', @level2name = N'Sequence';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The name of this node type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NodeTypes', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The plural name of this node type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NodeTypes', @level2type = N'COLUMN', @level2name = N'PluralName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The name of the slug image to use', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NodeTypes', @level2type = N'COLUMN', @level2name = N'SlugName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The color to use for the nodes of this type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NodeTypes', @level2type = N'COLUMN', @level2name = N'Color';

CREATE TABLE [dbo].[Nodes] (
    [NodeID]     INT           IDENTITY (1, 1) NOT NULL,
    [NodeTypeID] INT           NOT NULL,
    [Sequence]   INT           NOT NULL,
    [Title]      VARCHAR (150) NOT NULL,
    [Text]       VARCHAR (MAX) NOT NULL,
    [URL]        VARCHAR (250) NULL,
    PRIMARY KEY CLUSTERED ([NodeID] ASC),
    CONSTRAINT [FK_Nodes_NodeTypes] FOREIGN KEY ([NodeTypeID]) REFERENCES [dbo].[NodeTypes] ([NodeTypeID]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Nodes_NodeTypeSequence]
    ON [dbo].[Nodes]([NodeTypeID] ASC, [Sequence] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Autonumber identifier', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Nodes', @level2type = N'COLUMN', @level2name = N'NodeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reference to the node type, which in turn references the monad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Nodes', @level2type = N'COLUMN', @level2name = N'NodeTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The sequence this node appears in next to siblings', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Nodes', @level2type = N'COLUMN', @level2name = N'Sequence';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Title for this node', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Nodes', @level2type = N'COLUMN', @level2name = N'Title';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The text to display with this node', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Nodes', @level2type = N'COLUMN', @level2name = N'Text';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The optional link for this node', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Nodes', @level2type = N'COLUMN', @level2name = N'URL';

CREATE TABLE [dbo].[NodeLinks]
(
	[NodeLinkID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [NodeID1] INT NOT NULL, 
    [NodeID2] INT NOT NULL,
	[UniqueID] AS CASE WHEN [NodeID1] < [NodeID2] THEN CONVERT(varchar(10),[NodeID1]) ELSE CONVERT(varchar(10),[NodeID2]) END
		+ '/' + CASE WHEN [NodeID1] < [NodeID2] THEN CONVERT(varchar(10),[NodeID2]) ELSE CONVERT(varchar(10),[NodeID1]) END, 
    CONSTRAINT [FK_NodeLinks_Nodes1] FOREIGN KEY ([NodeID1]) REFERENCES [Nodes]([NodeID])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION,
	CONSTRAINT [FK_NodeLinks_Nodes2] FOREIGN KEY ([NodeID2]) REFERENCES [Nodes]([NodeID])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
)

GO



CREATE UNIQUE INDEX [UX_NodeLinks_UniqueID] ON [dbo].[NodeLinks] ([UniqueID])

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Autokey identifer',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'NodeLinks',
    @level2type = N'COLUMN',
    @level2name = N'NodeLinkID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The first node of the linking',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'NodeLinks',
    @level2type = N'COLUMN',
    @level2name = N'NodeID1'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The second node of the linking',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'NodeLinks',
    @level2type = N'COLUMN',
    @level2name = N'NodeID2'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'The unique identifer of the linked to prevent duplicates (computed)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'NodeLinks',
    @level2type = N'COLUMN',
    @level2name = N'UniqueID'