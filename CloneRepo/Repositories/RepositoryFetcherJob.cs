namespace CloneRepo.Repositories;

public class RepositoryFetcherJob
{
    private readonly RepositoryFetcher _repositoryFetcher;

    public RepositoryFetcherJob(RepositoryFetcher repositoryFetcher)
    {
        _repositoryFetcher = repositoryFetcher;
    }

}