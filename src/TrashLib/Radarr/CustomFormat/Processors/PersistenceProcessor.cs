﻿// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using TrashLib.Radarr.Config;
// using TrashLib.Radarr.CustomFormat.Api;
// using TrashLib.Radarr.CustomFormat.Models;
// using TrashLib.Radarr.CustomFormat.Models.Cache;
// using TrashLib.Radarr.CustomFormat.Processors.PersistenceSteps;
//
// namespace TrashLib.Radarr.CustomFormat.Processors
// {
//     public interface IPersistenceProcessorSteps
//     {
//         public IJsonTransactionStep JsonTransactionStep { get; }
//         public ICustomFormatApiPersistenceStep CustomFormatCustomFormatApiPersister { get; }
//         public IQualityProfileApiPersistenceStep ProfileQualityProfileApiPersister { get; }
//     }
//
//     internal class PersistenceProcessor : IPersistenceProcessor
//     {
//         private readonly Func<string, ICustomFormatService> _customFormatServiceFactory;
//         private readonly Func<string, IQualityProfileService> _qualityProfileServiceFactory;
//         private readonly Func<IPersistenceProcessorSteps> _stepsFactory;
//         private IPersistenceProcessorSteps _steps;
//
//         public PersistenceProcessor(
//             Func<string, ICustomFormatService> customFormatServiceFactory,
//             Func<string, IQualityProfileService> qualityProfileServiceFactory,
//             Func<IPersistenceProcessorSteps> stepsFactory)
//         {
//             _qualityProfileServiceFactory = qualityProfileServiceFactory;
//             _stepsFactory = stepsFactory;
//             _customFormatServiceFactory = customFormatServiceFactory;
//             _steps = _stepsFactory();
//         }
//
//         public CustomFormatTransactionData Transactions
//             => _steps.JsonTransactionStep.Transactions;
//
//         public IDictionary<string, List<UpdatedFormatScore>> UpdatedScores
//             => _steps.ProfileQualityProfileApiPersister.UpdatedScores;
//
//         public IReadOnlyCollection<string> InvalidProfileNames
//             => _steps.ProfileQualityProfileApiPersister.InvalidProfileNames;
//
//         public void Reset()
//         {
//             _steps = _stepsFactory();
//         }
//
//         public async Task PersistCustomFormats(
//             RadarrConfig config,
//             IEnumerable<ProcessedCustomFormatData> guideCfs,
//             IEnumerable<TrashIdMapping> deletedCfsInCache,
//             IDictionary<string, QualityProfileCustomFormatScoreMapping> profileScores)
//         {
//             var customFormatService = _customFormatServiceFactory(config.BuildUrl());
//             var radarrCfs = await customFormatService.GetCustomFormats();
//
//             // Step 1: Match CFs between the guide & Radarr and merge the data. The goal is to retain as much of the
//             // original data from Radarr as possible. There are many properties in the response JSON that we don't
//             // directly care about. We keep those and just update the ones we do care about.
//             _steps.JsonTransactionStep.Process(guideCfs, radarrCfs);
//
//             // Step 1.1: Optionally record deletions of custom formats in cache but not in the guide
//             if (config.DeleteOldCustomFormats)
//             {
//                 _steps.JsonTransactionStep.RecordDeletions(deletedCfsInCache, radarrCfs);
//             }
//
//             // Step 2: For each merged CF, persist it to Radarr via its API. This will involve a combination of updates
//             // to existing CFs and creation of brand new ones, depending on what's already available in Radarr.
//             await _steps.CustomFormatCustomFormatApiPersister.Process(
//                 customFormatService, _steps.JsonTransactionStep.Transactions);
//
//             // Step 3: Update all quality profiles with the scores from the guide for the uploaded custom formats
//             await _steps.ProfileQualityProfileApiPersister.Process(
//                 _qualityProfileServiceFactory(config.BuildUrl()), profileScores);
//         }
//     }
// }