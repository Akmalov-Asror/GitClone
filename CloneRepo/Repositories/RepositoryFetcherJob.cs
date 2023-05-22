﻿namespace CloneRepo.Repositories;

public class RepositoryFetcherJob
{
    private readonly RepositoryFetcher _repositoryFetcher;

    public RepositoryFetcherJob(RepositoryFetcher repositoryFetcher)
    {
        _repositoryFetcher = repositoryFetcher;
    }

    public async Task FetchRepositoriesJob(string owner, int count)
    {
        await _repositoryFetcher.FetchRepositories(owner, count);
    }
}