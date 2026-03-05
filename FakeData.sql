USE [Khdamatk];
GO
SET NOCOUNT ON;
GO

PRINT '=== Khdamatk Seed Data - Starting... ===';
GO

-- ============================================================
-- 1. Skills (100 records)
-- ============================================================
PRINT '1. Skills...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    INSERT INTO [dbo].[Skills] ([Name])
    VALUES (N'Skill_' + CAST(@i AS NVARCHAR(10)));
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 2. Categories (100 records)
-- ============================================================
PRINT '2. Categories...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    INSERT INTO [dbo].[Categories] ([Name], [Description])
    VALUES (
        N'Category_' + CAST(@i AS NVARCHAR(10)),
        N'Description for category number ' + CAST(@i AS NVARCHAR(10))
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 3. AspNetRoles (100 records)
-- ============================================================
PRINT '3. AspNetRoles...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @rId   NVARCHAR(450) = 'ROLE-' + RIGHT('000' + CAST(@i AS VARCHAR(10)), 3);
    DECLARE @rName NVARCHAR(256) = N'Role_' + CAST(@i AS NVARCHAR(10));
    INSERT INTO [dbo].[AspNetRoles] ([Id],[Name],[NormalizedName],[ConcurrencyStamp])
    VALUES (@rId, @rName, UPPER(@rName), CAST(NEWID() AS NVARCHAR(MAX)));
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 4. Medias (20 records فقط)
--    IDs سيكون 1-20 (IDENTITY)
-- ============================================================
PRINT '4. Medias (20 records)...';
DECLARE @i INT = 1;
WHILE @i <= 20
BEGIN
    INSERT INTO [dbo].[Medias]
        ([FileName],[ContentType],[Size],[FileExtension],[StoredFileName],[JobPostId],[JobOfferId])
    VALUES (
        N'photo_' + CAST(@i AS NVARCHAR(10)) + N'.jpg',
        N'image/jpeg',
        CAST(51200 + @i * 10240 AS BIGINT),
        N'.jpg',
        N'stored_' + RIGHT('00' + CAST(@i AS VARCHAR), 2)
            + N'_' + LEFT(REPLACE(CAST(NEWID() AS VARCHAR(36)),'-',''), 12) + N'.jpg',
        NULL, NULL
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 5. AspNetUsers (100 records)
--    ProfilePictureId: أول 20 مستخدم يحصلون على صورة، الباقي NULL
--    ServiceProviderProfileUserId: NULL مبدئياً (يُحدَّث بعد خطوة 6)
-- ============================================================
PRINT '5. AspNetUsers...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @uId    NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@i AS VARCHAR(10)), 3);
    DECLARE @uName  NVARCHAR(256) = N'user_' + CAST(@i AS NVARCHAR(10));
    DECLARE @uEmail NVARCHAR(256) = N'user_' + CAST(@i AS NVARCHAR(10)) + N'@khdamatk.com';
    DECLARE @picId  INT           = CASE WHEN @i <= 20 THEN @i ELSE NULL END;

    INSERT INTO [dbo].[AspNetUsers] (
        [Id],[DateOfBirth],[IsTrustedByAdmin],
        [UserName],[NormalizedUserName],
        [Email],[NormalizedEmail],
        [EmailConfirmed],[PasswordHash],[SecurityStamp],[ConcurrencyStamp],
        [PhoneNumber],[PhoneNumberConfirmed],[TwoFactorEnabled],
        [LockoutEnd],[LockoutEnabled],[AccessFailedCount],
        [ProfilePictureId],[ServiceProviderProfileUserId]
    )
    VALUES (
        @uId,
        DATEADD(YEAR, -(20 + @i % 38), GETUTCDATE()),
        CAST(CASE WHEN @i % 5 = 0 THEN 1 ELSE 0 END AS BIT),
        @uName, UPPER(@uName),
        @uEmail, UPPER(@uEmail),
        1,
        N'AQAAAAIAAYagAAAAEHash_' + @uId + N'PlaceholderHash==',
        CAST(NEWID() AS NVARCHAR(MAX)),
        CAST(NEWID() AS NVARCHAR(MAX)),
        N'+2010' + RIGHT('00000000' + CAST(@i * 1009 AS VARCHAR), 8),
        1, 0, NULL, 1, 0,
        @picId,
        NULL   -- سيُحدَّث لاحقاً
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 6. ServiceProviderProfiles (100 records - كل المستخدمين مزودو خدمة)
-- ============================================================
PRINT '6. ServiceProviderProfiles...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @uId NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@i AS VARCHAR(10)), 3);
    INSERT INTO [dbo].[ServiceProviderProfiles] (
        [UserId],[IsActive],[DateOfJoin],[LastActiveDate],[LastUpdate],
        [JobTitle],[Bio],
        [TotalReviews],[AverageRating],[ExperienceYears],
        [WorkingHoursPerWeek],[HourlyRate],[CompletedJobs],[IsAvailable],
        [FacebookUrl],[GithubUrl],[LinkedInUrl],[TwitterUrl]
    )
    VALUES (
        @uId,
        CAST(CASE WHEN @i % 10 <> 0 THEN 1 ELSE 0 END AS BIT),
        DATEADD(DAY, -(@i * 8), GETUTCDATE()),
        DATEADD(HOUR, -@i, GETUTCDATE()),
        DATEADD(HOUR, -@i, GETUTCDATE()),
        N'Professional Title ' + CAST(@i AS NVARCHAR(10)),
        N'Bio for provider #' + CAST(@i AS NVARCHAR(10)) + N'. Experienced and skilled professional offering quality services.',
        @i % 50,
        ROUND(3.0 + CAST(@i % 20 AS FLOAT) * 0.1, 1),
        1 + @i % 15,
        20.0 + CAST(@i % 20 AS FLOAT),
        10.0 + CAST(@i % 90 AS FLOAT),
        @i % 80,
        CAST(CASE WHEN @i % 4 <> 0 THEN 1 ELSE 0 END AS BIT),
        N'https://facebook.com/provider' + CAST(@i AS NVARCHAR(10)),
        N'https://github.com/provider' + CAST(@i AS NVARCHAR(10)),
        N'https://linkedin.com/in/provider' + CAST(@i AS NVARCHAR(10)),
        N'https://twitter.com/provider' + CAST(@i AS NVARCHAR(10))
    );
    SET @i = @i + 1;
END
GO

-- ربط ServiceProviderProfileUserId بعد إنشاء الـ profiles
PRINT '   Updating AspNetUsers.ServiceProviderProfileUserId...';
UPDATE [dbo].[AspNetUsers]
SET [ServiceProviderProfileUserId] = [Id]
WHERE [Id] LIKE 'USER-%';
GO

-- ============================================================
-- 7. VerificationData (100 records)
-- ============================================================
PRINT '7. VerificationData...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @uId     NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@i AS VARCHAR(10)), 3);
    DECLARE @country NVARCHAR(100) = CASE ((@i - 1) % 10)
        WHEN 0 THEN N'Egypt'        WHEN 1 THEN N'Saudi Arabia'
        WHEN 2 THEN N'UAE'          WHEN 3 THEN N'Jordan'
        WHEN 4 THEN N'Kuwait'       WHEN 5 THEN N'Bahrain'
        WHEN 6 THEN N'Oman'         WHEN 7 THEN N'Qatar'
        WHEN 8 THEN N'Lebanon'      ELSE      N'Morocco'
    END;
    INSERT INTO [dbo].[VerificationData] ([UserId],[NationalNumber],[Country],[City],[Status])
    VALUES (
        @uId,
        RIGHT('00000000000000' + CAST(@i * 123457 AS VARCHAR(20)), 14),
        @country,
        N'City_' + CAST(@i AS NVARCHAR(10)),
        CASE @i % 3 WHEN 0 THEN N'Verified' WHEN 1 THEN N'Pending' ELSE N'Rejected' END
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 8. VerificationsCodes (100 records)
-- ============================================================
PRINT '8. VerificationsCodes...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @uId   NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@i AS VARCHAR(10)), 3);
    DECLARE @vType NVARCHAR(MAX) = CASE @i % 3
        WHEN 0 THEN N'EmailVerification'
        WHEN 1 THEN N'PasswordReset'
        ELSE         N'PhoneVerification'
    END;
    INSERT INTO [dbo].[VerificationsCodes]
        ([Type],[Value],[IsUsed],[UserId],[Createdat],[CreatedBy],[Updatedat],[UpdatedBy],[IsDelete])
    VALUES (
        @vType,
        100000 + ((@i * 7919) % 900000),
        CAST(@i % 2 AS BIT),
        @uId,
        DATEADD(HOUR, -@i, GETUTCDATE()),
        @uId,
        NULL, NULL, 0
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 9. RefreshTokens (100 records)
-- ============================================================
PRINT '9. RefreshTokens...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @uId NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@i AS VARCHAR(10)), 3);
    INSERT INTO [dbo].[RefreshTokens]
        ([Token],[ExpireAt],[CreatedAt],[UsedAt],[IsUsed],[RevokedAt],[IsDeleted],[UserId])
    VALUES (
        LEFT(
            REPLACE(CAST(NEWID() AS VARCHAR(36)),'-','') +
            REPLACE(CAST(NEWID() AS VARCHAR(36)),'-',''), 64
        ),
        DATEADD(DAY, 30, GETUTCDATE()),
        DATEADD(DAY, -@i, GETUTCDATE()),
        CASE WHEN @i % 3 = 0 THEN DATEADD(DAY, -1, GETUTCDATE()) ELSE NULL END,
        CAST(CASE WHEN @i % 3 = 0 THEN 1 ELSE 0 END AS BIT),
        NULL, 0,
        @uId
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 10. CreditCards (100 records) - بطاقة واحدة لكل مستخدم
-- ============================================================
PRINT '10. CreditCards...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @uId      NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@i AS VARCHAR(10)), 3);
    DECLARE @cardType NVARCHAR(MAX) = CASE @i % 4
        WHEN 0 THEN N'Visa' WHEN 1 THEN N'Mastercard'
        WHEN 2 THEN N'Amex' ELSE         N'Discover'
    END;
    INSERT INTO [dbo].[CreditCards]
        ([Tokenized],[Last4Digits],[ExpirationDate],[CardType],[UserId])
    VALUES (
        N'tok_' + REPLACE(CAST(NEWID() AS NVARCHAR(36)), N'-', N''),
        RIGHT(CAST(1000 + @i AS VARCHAR(10)), 4),
        DATEADD(YEAR, 2 + @i % 5, GETUTCDATE()),
        @cardType,
        @uId
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 11. AspNetRoleClaims (100 records)
-- ============================================================
PRINT '11. AspNetRoleClaims...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @rId NVARCHAR(450) = 'ROLE-' + RIGHT('000' + CAST(@i AS VARCHAR(10)), 3);
    INSERT INTO [dbo].[AspNetRoleClaims] ([RoleId],[ClaimType],[ClaimValue])
    VALUES (
        @rId,
        N'Permission',
        N'Permission.Feature.' + CAST(@i AS NVARCHAR(10))
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 12. AspNetUserClaims (100 records)
-- ============================================================
PRINT '12. AspNetUserClaims...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @uId NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@i AS VARCHAR(10)), 3);
    INSERT INTO [dbo].[AspNetUserClaims] ([UserId],[ClaimType],[ClaimValue])
    VALUES (
        @uId,
        N'UserClaim',
        N'ClaimValue_' + CAST(@i AS NVARCHAR(10))
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 13. AspNetUserLogins (100 records)
--     PK: (LoginProvider, ProviderKey) - يجب أن تكون فريدة
--     ProviderKey فريد لكل صف
-- ============================================================
PRINT '13. AspNetUserLogins...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @uId      NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@i AS VARCHAR(10)), 3);
    DECLARE @provider NVARCHAR(450) = CASE @i % 4
        WHEN 0 THEN N'Google' WHEN 1 THEN N'Facebook'
        WHEN 2 THEN N'Microsoft' ELSE  N'Apple'
    END;
    INSERT INTO [dbo].[AspNetUserLogins]
        ([LoginProvider],[ProviderKey],[ProviderDisplayName],[UserId])
    VALUES (
        @provider,
        N'ProvKey_' + CAST(@i AS NVARCHAR(10)),  -- فريد لكل صف
        @provider + N' Account',
        @uId
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 14. AspNetUserRoles (100 records)
--     PK: (UserId, RoleId)  - مستخدم i → دور i
-- ============================================================
PRINT '14. AspNetUserRoles...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @uId NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@i AS VARCHAR(10)), 3);
    DECLARE @rId NVARCHAR(450) = 'ROLE-' + RIGHT('000' + CAST(@i AS VARCHAR(10)), 3);
    INSERT INTO [dbo].[AspNetUserRoles] ([UserId],[RoleId])
    VALUES (@uId, @rId);
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 15. AspNetUserTokens (100 records)
--     PK: (UserId, LoginProvider, Name)
-- ============================================================
PRINT '15. AspNetUserTokens...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @uId NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@i AS VARCHAR(10)), 3);
    INSERT INTO [dbo].[AspNetUserTokens]
        ([UserId],[LoginProvider],[Name],[Value])
    VALUES (
        @uId,
        N'LocalLoginProvider',
        N'RefreshToken_' + CAST(@i AS NVARCHAR(10)),
        N'tokval_' + REPLACE(CAST(NEWID() AS NVARCHAR(36)), N'-', N'')
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 16. Services (100 records)
--     المزود: USER-001 لـ services 1-2، USER-002 لـ 3-4، إلخ
--     MainMediaId = NULL لتجنب مشكلة الـ UNIQUE constraint
-- ============================================================
PRINT '16. Services...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @provIdx INT           = ((@i - 1) / 2) + 1;
    DECLARE @provId  NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@provIdx AS VARCHAR(10)), 3);
    DECLARE @catId   INT           = ((@i - 1) % 100) + 1;

    INSERT INTO [dbo].[Services] (
        [Title],[ShortDescription],[Price],[DeliveryTimeInDays],
        [AverageRating],[TotalReviews],[CategoryId],[ServiceProviderProfileId],
        [MainMediaId],[Createdat],[CreatedBy],[Updatedat],[UpdatedBy],[IsDelete],
        [Concepts],[DeliverTimeInDays],[DetailedDescription],[RevisionCount]
    )
    VALUES (
        N'Service Title ' + CAST(@i AS NVARCHAR(10)),
        N'Short description for service #' + CAST(@i AS NVARCHAR(10)),
        ROUND(CAST(20 + @i * 3 AS DECIMAL(18,2)), 2),
        1 + @i % 30,
        ROUND(3.0 + CAST(@i % 20 AS FLOAT) * 0.1, 1),
        @i % 50,
        @catId,
        @provId,
        NULL,   -- MainMediaId = NULL (Unique constraint)
        DATEADD(DAY, -@i, GETUTCDATE()),
        @provId,
        NULL, NULL, 0,
        N'[]',
        '00:00:00',
        N'Detailed description for service #' + CAST(@i AS NVARCHAR(10)) + N'. Premium quality guaranteed.',
        @i % 5
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 17. JobPosts (100 records)
--     CustomerId: المستخدمون 51-100 كعملاء
-- ============================================================
PRINT '17. JobPosts...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @clientIdx INT           = 51 + ((@i - 1) % 50);
    DECLARE @clientId  NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@clientIdx AS VARCHAR(10)), 3);
    DECLARE @catId     INT           = ((@i - 1) % 100) + 1;

    INSERT INTO [dbo].[JobPosts] (
        [CustomerId],[CategoryId],[Title],[Description],
        [BudgetMin],[BudgetMax],[Status],[Deadline],[CreatedAt],
        [ExperienceLevel],[ProjectLength],[TimeCommitment]
    )
    VALUES (
        @clientId, @catId,
        N'Job Post #' + CAST(@i AS NVARCHAR(10)),
        N'Looking for a professional for task #' + CAST(@i AS NVARCHAR(10)) + N'. Must have relevant experience.',
        CAST(50 + @i * 5 AS DECIMAL(18,2)),
        CAST(200 + @i * 10 AS DECIMAL(18,2)),
        CASE @i % 4
            WHEN 0 THEN N'Open' WHEN 1 THEN N'Closed'
            WHEN 2 THEN N'InProgress' ELSE N'Completed'
        END,
        DATEADD(DAY, 30 + @i % 60, GETUTCDATE()),
        DATEADD(DAY, -@i, GETUTCDATE()),
        CASE @i % 3 WHEN 0 THEN N'Entry' WHEN 1 THEN N'Intermediate' ELSE N'Expert' END,
        CASE @i % 3 WHEN 0 THEN N'Short' WHEN 1 THEN N'Medium' ELSE N'Long' END,
        CASE @i % 3 WHEN 0 THEN N'FullTime' WHEN 1 THEN N'PartTime' ELSE N'Flexible' END
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 18. JobSkillRequirements (100 records)
--     PK: (JobPostId, SkillId) - Job i → Skill i (فريد تماماً)
-- ============================================================
PRINT '18. JobSkillRequirements...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    INSERT INTO [dbo].[JobSkillRequirements] ([JobPostId],[SkillId],[RequiredLevel])
    VALUES (
        @i, @i,
        CASE @i % 3 WHEN 0 THEN N'Expert' WHEN 1 THEN N'Intermediate' ELSE N'Beginner' END
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 19. serviceOrders (100 records)
--     UserID (عميل): مستخدمون 51-100
--     ServiceID: خدمات 1-100
--     ServiceProviderId: يطابق مزود الخدمة
-- ============================================================
PRINT '19. serviceOrders...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @clientIdx INT           = 51 + ((@i - 1) % 50);
    DECLARE @clientId  NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@clientIdx AS VARCHAR(10)), 3);
    DECLARE @provIdx   INT           = ((@i - 1) / 2) + 1;
    DECLARE @provId    NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@provIdx AS VARCHAR(10)), 3);

    INSERT INTO [dbo].[serviceOrders] (
        [UserID],[ServiceID],[ServiceProviderId],[Amount],[Status],
        [CompletionDate],[AdditionalDetails],
        [Createdat],[CreatedBy],[Updatedat],[UpdatedBy],[IsDelete],
        [invoiceId],[invoiceKey]
    )
    VALUES (
        @clientId, @i, @provId,
        ROUND(CAST(50 + @i * 5 AS DECIMAL(18,2)), 2),
        CASE @i % 5
            WHEN 0 THEN N'Pending'    WHEN 1 THEN N'Active'
            WHEN 2 THEN N'Completed'  WHEN 3 THEN N'Cancelled'
            ELSE         N'InReview'
        END,
        CASE WHEN @i % 5 = 2 THEN DATEADD(DAY, -(@i % 20), GETUTCDATE()) ELSE NULL END,
        N'Additional details for order #' + CAST(@i AS NVARCHAR(10)),
        DATEADD(DAY, -@i, GETUTCDATE()),
        @clientId,
        NULL, NULL, 0,
        @i,
        N'INV-' + RIGHT('00000' + CAST(@i AS VARCHAR(10)), 5)
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 20. Conversations (100 records)
--     ServiceOrderId: UNIQUE → محادثة واحدة لكل طلب (أوردر 1-100)
-- ============================================================
PRINT '20. Conversations...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @clientIdx INT           = 51 + ((@i - 1) % 50);
    DECLARE @clientId  NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@clientIdx AS VARCHAR(10)), 3);
    DECLARE @provIdx   INT           = ((@i - 1) / 2) + 1;
    DECLARE @provId    NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@provIdx AS VARCHAR(10)), 3);

    INSERT INTO [dbo].[Conversations] (
        [Title],[ServiceOrderId],[ClientId],[Category],[ProviderId],
        [Createdat],[CreatedBy],[Updatedat],[UpdatedBy],[IsDelete],
        [ContextType],[RelatedEntityId]
    )
    VALUES (
        N'Conversation for Order #' + CAST(@i AS NVARCHAR(10)),
        @i,
        @clientId,
        N'General',
        @provId,
        DATEADD(DAY, -@i, GETUTCDATE()),
        @clientId,
        NULL, NULL, 0,
        N'ServiceOrder',
        @i
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 21. jobOffers (100 records)
--     JobPostId: 1-100
--     ProviderProfileId: يدور على المزودين 1-100
--     ConversationId: NULL (nullable)
-- ============================================================
PRINT '21. jobOffers...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @provIdx INT           = ((@i - 1) % 100) + 1;
    DECLARE @provId  NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@provIdx AS VARCHAR(10)), 3);

    INSERT INTO [dbo].[jobOffers] (
        [ExperienceLevel],[ProposedPrice],[DeliveryTimeInDays],
        [Status],[JobPostId],[ProviderProfileId],[ConversationId],
        [IsAccepted],[NetAmount],[Deadline],[Description],[SimilarWorkExamplesURL],[TimeCommitment]
    )
    VALUES (
        CASE @i % 3 WHEN 0 THEN N'Entry' WHEN 1 THEN N'Intermediate' ELSE N'Expert' END,
        ROUND(CAST(100 + @i * 7 AS DECIMAL(18,2)), 2),
        3 + @i % 27,
        CASE @i % 4
            WHEN 0 THEN N'Pending'   WHEN 1 THEN N'Accepted'
            WHEN 2 THEN N'Rejected'  ELSE         N'Withdrawn'
        END,
        @i,
        @provId,
        NULL,
        CAST(CASE WHEN @i % 4 = 1 THEN 1 ELSE 0 END AS BIT),
        ROUND(CAST(90 + @i * 6 AS DECIMAL(18,2)), 2),
        DATEADD(DAY, 14 + @i % 30, GETUTCDATE()),
        N'Offer description for job #' + CAST(@i AS NVARCHAR(10)) + N'. Committed to delivering excellent results.',
        N'https://portfolio.example.com/work/' + CAST(@i AS NVARCHAR(10)),
        CASE @i % 3 WHEN 0 THEN N'FullTime' WHEN 1 THEN N'PartTime' ELSE N'Flexible' END
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 22. Messages (100 records)
-- ============================================================
PRINT '22. Messages...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @convId    INT           = ((@i - 1) % 100) + 1;
    DECLARE @clientIdx INT           = 51 + ((@i - 1) % 50);
    DECLARE @provIdx   INT           = ((@i - 1) / 2) + 1;
    DECLARE @senderId  NVARCHAR(450) = CASE WHEN @i % 2 = 0
        THEN 'USER-' + RIGHT('000' + CAST(@clientIdx AS VARCHAR(10)), 3)
        ELSE 'USER-' + RIGHT('000' + CAST(@provIdx   AS VARCHAR(10)), 3)
    END;

    INSERT INTO [dbo].[Messages] (
        [ConversationId],[SenderId],[Content],[IsRead],[ServiceOrderId],
        [Createdat],[CreatedBy],[Updatedat],[UpdatedBy],[IsDelete]
    )
    VALUES (
        @convId,
        @senderId,
        N'Message #' + CAST(@i AS NVARCHAR(10)) + N': Hello, I am following up on the service order. Please let me know the progress.',
        CAST(@i % 2 AS BIT),
        @convId,   -- ServiceOrderId يطابق رقم المحادثة
        DATEADD(MINUTE, -@i * 15, GETUTCDATE()),
        @senderId,
        NULL, NULL, 0
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 23. Reviews (100 records)
--     UNIQUE على OrderId → مراجعة واحدة لكل طلب
-- ============================================================
PRINT '23. Reviews...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @clientIdx INT           = 51 + ((@i - 1) % 50);
    DECLARE @clientId  NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@clientIdx AS VARCHAR(10)), 3);
    DECLARE @provIdx   INT           = ((@i - 1) / 2) + 1;
    DECLARE @provId    NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@provIdx AS VARCHAR(10)), 3);

    INSERT INTO [dbo].[Reviews] (
        [Title],[Content],[Rating],[OrderId],[ReviewerId],[ServiceProviderId],
        [Createdat],[CreatedBy],[Updatedat],[UpdatedBy],[IsDelete]
    )
    VALUES (
        N'Review #' + CAST(@i AS NVARCHAR(10)),
        N'Great professional work delivered for order #' + CAST(@i AS NVARCHAR(10)) + N'. Highly recommended.',
        ROUND(CAST(3 AS FLOAT) + CAST(@i % 20 AS FLOAT) * 0.1, 1),
        @i,
        @clientId,
        @provId,
        DATEADD(DAY, -(@i % 25), GETUTCDATE()),
        @clientId,
        NULL, NULL, 0
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 24. PaymentTransactions (100 records)
--     UNIQUE على OrderId → دفعة واحدة لكل طلب
-- ============================================================
PRINT '24. PaymentTransactions...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @amount    DECIMAL(18,2) = ROUND(CAST(50 + @i * 5 AS DECIMAL(18,2)), 2);
    DECLARE @platFee   DECIMAL(18,2) = ROUND(@amount * 0.10, 2);
    DECLARE @netPayout DECIMAL(18,2) = @amount - @platFee;

    INSERT INTO [dbo].[PaymentTransactions] (
        [OrderId],[Amount],[PlatformFee],[NetPayout],[Currency],[Status],
        [TransactionDate],[GatewayUsed],[GatewayReferenceId],
        [TokenizedCreditCardId],[CreditCardId]
    )
    VALUES (
        @i,
        @amount, @platFee, @netPayout,
        N'USD',
        CASE @i % 3 WHEN 0 THEN N'Completed' WHEN 1 THEN N'Pending' ELSE N'Failed' END,
        DATEADD(DAY, -@i, GETUTCDATE()),
        CASE @i % 3 WHEN 0 THEN N'Stripe' WHEN 1 THEN N'PayPal' ELSE N'Fawry' END,
        N'GW_' + CAST(@i AS NVARCHAR(10)) + N'_' +
            LEFT(REPLACE(CAST(NEWID() AS VARCHAR(36)),'-',''), 10),
        NULL,
        @i   -- CreditCardId 1-100 (كل مستخدم له بطاقة)
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 25. Disputes (50 records)
--     RaiserConversationId UNIQUE: محادثات 1-50
--     TargetConversationId UNIQUE: محادثات 51-100
--     كل محادثة تُستخدم مرة واحدة فقط في كل عمود
-- ============================================================
PRINT '25. Disputes (50 records)...';
DECLARE @i INT = 1;
WHILE @i <= 50
BEGIN
    DECLARE @clientIdx    INT           = 51 + ((@i - 1) % 50);
    DECLARE @raiserId     NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@clientIdx AS VARCHAR(10)), 3);
    DECLARE @provIdx      INT           = ((@i - 1) / 2) + 1;
    DECLARE @targetId     NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@provIdx AS VARCHAR(10)), 3);
    DECLARE @raiserConvId INT           = @i;
    DECLARE @targetConvId INT           = @i + 50;

    INSERT INTO [dbo].[Disputes] (
        [ServiceOrderId],[RaiserId],[TargetId],[AdminReviewerId],
        [RaiserConversationId],[TargetConversationId],
        [Status],[Type],[AmountUnderDispute],
        [ReasonDetails],[FinalDecisionDetails],
        [IsDecisionAcceptedByRaiser],[IsDecisionAcceptedByTarget],
        [OpenedDate],[ResolutionDate],
        [Createdat],[CreatedBy],[Updatedat],[UpdatedBy],[IsDelete]
    )
    VALUES (
        @i,
        @raiserId,
        @targetId,
        N'USER-001',   -- المشرف الإداري
        @raiserConvId,
        @targetConvId,
        CASE @i % 4
            WHEN 0 THEN N'Open'        WHEN 1 THEN N'UnderReview'
            WHEN 2 THEN N'Resolved'    ELSE         N'Closed'
        END,
        CASE @i % 3
            WHEN 0 THEN N'QualityIssue'
            WHEN 1 THEN N'NonDelivery'
            ELSE         N'PaymentDispute'
        END,
        ROUND(CAST(50 + @i * 4 AS DECIMAL(18,2)), 2),
        N'Reason for dispute #' + CAST(@i AS NVARCHAR(10)) + N'. Client claims service was not as described.',
        CASE WHEN @i % 4 = 2 THEN N'Dispute resolved. Partial refund issued.' ELSE NULL END,
        CASE WHEN @i % 4 = 2 THEN CAST(1 AS BIT)  ELSE NULL END,
        CASE WHEN @i % 4 = 2 THEN CAST(0 AS BIT)  ELSE NULL END,
        DATEADD(DAY, -@i, GETUTCDATE()),
        CASE WHEN @i % 4 = 2 THEN DATEADD(DAY, -(@i/2), GETUTCDATE()) ELSE NULL END,
        DATEADD(DAY, -@i, GETUTCDATE()),
        @raiserId,
        NULL, NULL, 0
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 26. PortfolioItems (100 records)
-- ============================================================
PRINT '26. PortfolioItems...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @provIdx INT           = ((@i - 1) % 100) + 1;
    DECLARE @provId  NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@provIdx AS VARCHAR(10)), 3);

    INSERT INTO [dbo].[PortfolioItems] (
        [ServiceProviderProfileId],[Title],[Description],[ProjectUrl],[CompletionDate],
        [Createdat],[CreatedBy],[Updatedat],[UpdatedBy],[IsDelete],
        [Company],[Degree],[EndDate],[FieldOfStudy],[SchoolName],[StartDate]
    )
    VALUES (
        @provId,
        N'Portfolio Item ' + CAST(@i AS NVARCHAR(10)),
        N'Description of project #' + CAST(@i AS NVARCHAR(10)) + N'. Demonstrates expertise and delivers real value.',
        N'https://portfolio.example.com/project/' + CAST(@i AS NVARCHAR(10)),
        DATEADD(MONTH, -(@i % 24), GETUTCDATE()),
        DATEADD(DAY, -@i, GETUTCDATE()),
        @provId,
        NULL, NULL, 0,
        N'Company_' + CAST(@i AS NVARCHAR(10)),
        CASE @i % 4
            WHEN 0 THEN N'Bachelor' WHEN 1 THEN N'Master'
            WHEN 2 THEN N'PhD'      ELSE         N'Diploma'
        END,
        DATEADD(YEAR, -(@i % 8), GETUTCDATE()),
        N'Computer Science',
        N'University_' + CAST(@i AS NVARCHAR(10)),
        DATEADD(YEAR, -(@i % 8) - 4, GETUTCDATE())
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 27. PortfolioMedia (100 records)
--     PK: (PortfolioItemId, MediaId) - يدور على IDs الـ media 1-20
--     الأزواج فريدة: (1,1),(2,2),...,(20,20),(21,1),(22,2),...
-- ============================================================
PRINT '27. PortfolioMedia...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @mediaId INT = ((@i - 1) % 20) + 1;
    INSERT INTO [dbo].[PortfolioMedia] ([PortfolioItemId],[MediaId])
    VALUES (@i, @mediaId);
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 28. Certificates (100 records)
--     MediaId = NULL لتجنب مشكلة الـ UNIQUE constraint
--     (عندنا 20 صورة فقط وكلها ممكن تتعارض)
-- ============================================================
PRINT '28. Certificates...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @provIdx INT           = ((@i - 1) % 100) + 1;
    DECLARE @provId  NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@provIdx AS VARCHAR(10)), 3);

    INSERT INTO [dbo].[Certificates] (
        [ServiceProviderProfileId],[Title],[Issuer],[Type],[YearAcquired],
        [MediaId],[Createdat],[CreatedBy],[Updatedat],[UpdatedBy],[IsDelete]
    )
    VALUES (
        @provId,
        N'Certificate_' + CAST(@i AS NVARCHAR(10)),
        CASE @i % 5
            WHEN 0 THEN N'Coursera'   WHEN 1 THEN N'Udemy'
            WHEN 2 THEN N'Microsoft'  WHEN 3 THEN N'Google'
            ELSE         N'AWS'
        END,
        CASE @i % 3
            WHEN 0 THEN N'Technical' WHEN 1 THEN N'Professional' ELSE N'Academic'
        END,
        2015 + @i % 10,
        NULL,   -- MediaId = NULL
        DATEADD(DAY, -@i, GETUTCDATE()),
        @provId,
        NULL, NULL, 0
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 29. ProviderSkills (100 records)
--     PK: (ServiceProviderProfileId, SkillId)
--     Provider i → Skill i (تحديد فريد لكل صف)
-- ============================================================
PRINT '29. ProviderSkills...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @provId NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@i AS VARCHAR(10)), 3);
    INSERT INTO [dbo].[ProviderSkills] ([ServiceProviderProfileId],[SkillId],[MyLevel])
    VALUES (
        @provId,
        @i,
        CASE @i % 3 WHEN 0 THEN N'Expert' WHEN 1 THEN N'Intermediate' ELSE N'Beginner' END
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 30. UserFavorites (100 records)
--     PK: (User_FK_ID, ServiceID)
--     User i يُفضّل Service i
-- ============================================================
PRINT '30. UserFavorites...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @uId NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@i AS VARCHAR(10)), 3);
    INSERT INTO [dbo].[UserFavorites] ([User_FK_ID],[ServiceID],[AddedTime],[UserId])
    VALUES (
        @uId,
        @i,
        DATEADD(DAY, -@i, GETUTCDATE()),
        @uId
    );
    SET @i = @i + 1;
END
GO

-- ============================================================
-- 31. ServiceMedia (100 records)
--     PK: (ServiceId, MediaId) - يدور على media 1-20
--     الأزواج فريدة: (1,1),(2,2),...,(20,20),(21,1),(22,2),...
-- ============================================================
PRINT '31. ServiceMedia...';
DECLARE @i INT = 1;
WHILE @i <= 100
BEGIN
    DECLARE @mediaId INT = ((@i - 1) % 20) + 1;
    INSERT INTO [dbo].[ServiceMedia] ([ServiceId],[MediaId])
    VALUES (@i, @mediaId);
    SET @i = @i + 1;
END
GO

-- ============================================================
-- التحقق النهائي من عدد السجلات
-- ============================================================
PRINT '=== Final Record Count Verification ===';
SELECT 'Skills'                  AS TableName, COUNT(*) AS RecordCount FROM [dbo].[Skills]
UNION ALL SELECT 'Categories',                 COUNT(*) FROM [dbo].[Categories]
UNION ALL SELECT 'AspNetRoles',                COUNT(*) FROM [dbo].[AspNetRoles]
UNION ALL SELECT 'Medias',                     COUNT(*) FROM [dbo].[Medias]
UNION ALL SELECT 'AspNetUsers',                COUNT(*) FROM [dbo].[AspNetUsers]
UNION ALL SELECT 'ServiceProviderProfiles',    COUNT(*) FROM [dbo].[ServiceProviderProfiles]
UNION ALL SELECT 'VerificationData',           COUNT(*) FROM [dbo].[VerificationData]
UNION ALL SELECT 'VerificationsCodes',         COUNT(*) FROM [dbo].[VerificationsCodes]
UNION ALL SELECT 'RefreshTokens',              COUNT(*) FROM [dbo].[RefreshTokens]
UNION ALL SELECT 'CreditCards',                COUNT(*) FROM [dbo].[CreditCards]
UNION ALL SELECT 'AspNetRoleClaims',           COUNT(*) FROM [dbo].[AspNetRoleClaims]
UNION ALL SELECT 'AspNetUserClaims',           COUNT(*) FROM [dbo].[AspNetUserClaims]
UNION ALL SELECT 'AspNetUserLogins',           COUNT(*) FROM [dbo].[AspNetUserLogins]
UNION ALL SELECT 'AspNetUserRoles',            COUNT(*) FROM [dbo].[AspNetUserRoles]
UNION ALL SELECT 'AspNetUserTokens',           COUNT(*) FROM [dbo].[AspNetUserTokens]
UNION ALL SELECT 'Services',                   COUNT(*) FROM [dbo].[Services]
UNION ALL SELECT 'JobPosts',                   COUNT(*) FROM [dbo].[JobPosts]
UNION ALL SELECT 'JobSkillRequirements',       COUNT(*) FROM [dbo].[JobSkillRequirements]
UNION ALL SELECT 'serviceOrders',              COUNT(*) FROM [dbo].[serviceOrders]
UNION ALL SELECT 'Conversations',              COUNT(*) FROM [dbo].[Conversations]
UNION ALL SELECT 'jobOffers',                  COUNT(*) FROM [dbo].[jobOffers]
UNION ALL SELECT 'Messages',                   COUNT(*) FROM [dbo].[Messages]
UNION ALL SELECT 'Reviews',                    COUNT(*) FROM [dbo].[Reviews]
UNION ALL SELECT 'PaymentTransactions',        COUNT(*) FROM [dbo].[PaymentTransactions]
UNION ALL SELECT 'Disputes',                   COUNT(*) FROM [dbo].[Disputes]
UNION ALL SELECT 'PortfolioItems',             COUNT(*) FROM [dbo].[PortfolioItems]
UNION ALL SELECT 'PortfolioMedia',             COUNT(*) FROM [dbo].[PortfolioMedia]
UNION ALL SELECT 'Certificates',               COUNT(*) FROM [dbo].[Certificates]
UNION ALL SELECT 'ProviderSkills',             COUNT(*) FROM [dbo].[ProviderSkills]
UNION ALL SELECT 'UserFavorites',              COUNT(*) FROM [dbo].[UserFavorites]
UNION ALL SELECT 'ServiceMedia',               COUNT(*) FROM [dbo].[ServiceMedia]
ORDER BY TableName;
GO

PRINT '=== Done! All seed data inserted successfully. ===';
GO


USE [Khdamatk];
GO
SET NOCOUNT ON;
GO

-- ============================================================
-- إصلاح: JobSkillRequirements
-- ============================================================
PRINT 'Fix: JobSkillRequirements...';

-- حذف أي سجلات ناجحة من المحاولة السابقة
DELETE FROM [dbo].[JobSkillRequirements];

DECLARE @jp_ids TABLE (RowNum INT IDENTITY(1,1), JobPostId INT);
INSERT INTO @jp_ids (JobPostId)
SELECT [Id] FROM [dbo].[JobPosts] ORDER BY [Id];

DECLARE @total INT = (SELECT COUNT(*) FROM @jp_ids);
DECLARE @i    INT = 1;

WHILE @i <= @total
BEGIN
    DECLARE @jpId   INT = (SELECT JobPostId FROM @jp_ids WHERE RowNum = @i);
    DECLARE @skillId INT = ((@i - 1) % 100) + 1;

    INSERT INTO [dbo].[JobSkillRequirements] ([JobPostId],[SkillId],[RequiredLevel])
    VALUES (
        @jpId,
        @skillId,
        CASE @i % 3 WHEN 0 THEN N'Expert' WHEN 1 THEN N'Intermediate' ELSE N'Beginner' END
    );
    SET @i = @i + 1;
END

PRINT 'JobSkillRequirements done: ' + CAST(@@ROWCOUNT AS VARCHAR) + ' rows';
GO

-- ============================================================
-- إصلاح: jobOffers
-- ============================================================
PRINT 'Fix: jobOffers...';

-- حذف أي سجلات ناجحة من المحاولة السابقة
DELETE FROM [dbo].[jobOffers];

DECLARE @jp_ids TABLE (RowNum INT IDENTITY(1,1), JobPostId INT);
INSERT INTO @jp_ids (JobPostId)
SELECT [Id] FROM [dbo].[JobPosts] ORDER BY [Id];

DECLARE @total INT = (SELECT COUNT(*) FROM @jp_ids);
DECLARE @i    INT = 1;

WHILE @i <= @total
BEGIN
    DECLARE @jpId    INT           = (SELECT JobPostId FROM @jp_ids WHERE RowNum = @i);
    DECLARE @provIdx INT           = ((@i - 1) % 100) + 1;
    DECLARE @provId  NVARCHAR(450) = 'USER-' + RIGHT('000' + CAST(@provIdx AS VARCHAR(10)), 3);

    INSERT INTO [dbo].[jobOffers] (
        [ExperienceLevel],[ProposedPrice],[DeliveryTimeInDays],
        [Status],[JobPostId],[ProviderProfileId],[ConversationId],
        [IsAccepted],[NetAmount],[Deadline],[Description],
        [SimilarWorkExamplesURL],[TimeCommitment]
    )
    VALUES (
        CASE @i % 3 WHEN 0 THEN N'Entry' WHEN 1 THEN N'Intermediate' ELSE N'Expert' END,
        ROUND(CAST(100 + @i * 7 AS DECIMAL(18,2)), 2),
        3 + @i % 27,
        CASE @i % 4
            WHEN 0 THEN N'Pending'  WHEN 1 THEN N'Accepted'
            WHEN 2 THEN N'Rejected' ELSE         N'Withdrawn'
        END,
        @jpId,
        @provId,
        NULL,
        CAST(CASE WHEN @i % 4 = 1 THEN 1 ELSE 0 END AS BIT),
        ROUND(CAST(90 + @i * 6 AS DECIMAL(18,2)), 2),
        DATEADD(DAY, 14 + @i % 30, GETUTCDATE()),
        N'Offer description for job #' + CAST(@i AS NVARCHAR(10)) + N'. Committed to delivering excellent results.',
        N'https://portfolio.example.com/work/' + CAST(@i AS NVARCHAR(10)),
        CASE @i % 3 WHEN 0 THEN N'FullTime' WHEN 1 THEN N'PartTime' ELSE N'Flexible' END
    );
    SET @i = @i + 1;
END

PRINT 'jobOffers done: ' + CAST(@@ROWCOUNT AS VARCHAR) + ' rows';
GO

-- ============================================================
-- التحقق النهائي
-- ============================================================
SELECT 'JobPosts'             AS TableName, COUNT(*) AS RecordCount FROM [dbo].[JobPosts]
UNION ALL
SELECT 'JobSkillRequirements',              COUNT(*) FROM [dbo].[JobSkillRequirements]
UNION ALL
SELECT 'jobOffers',                         COUNT(*) FROM [dbo].[jobOffers]
ORDER BY TableName;
GO