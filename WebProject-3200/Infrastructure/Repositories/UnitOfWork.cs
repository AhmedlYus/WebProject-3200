using WebProject_3200.Application.Interfaces;
// we need to also use the entityFrameWrok Storage. 
//for now its uncommented
using Microsoft.entityFrameWrok.Storage;

namespace WebProject_3200.Infrastructure.Repositories;

/*
The unit of work needs to implement all the functionalities
from its interface contract. 

Unit of Work Pattern : created to ensure data consistancy and
integrity by grouping database requests and operations into a units.
that can be commited, or disposed. if it affects the db in negative way we can dispose of the transactions.

Keeping track of objects/business trasactions that can affect the database.
Hepls optimize preformance.

*/


public class UnitOfWork : IUnitOfWork {
    // add the database connection here, as readonly
    private readonly DatabaseContext _context;
    // and context transaction.
    private IDbContextTransaction? _transaction;

    // add all the repositories here as interface - Repository
    // dependancy injections. 
    // Public IplayerRepository PlayerRepository {get; }

    public UnitOfWork(DatabaseContext context) {
        _context = context
        // add the repositories in construtor and assign them new instaces.
        // PlayerRepository = new PlayerRepository(_context);
    }

    // save changes;
    public async Task SaveAsync() {

        // when saving consolelog to see if it is working
        Console.WriteLine("UnitOfWork Started");

        // when the db context is created we save the data.
        return await _context.SaveChangesAsync();
    }

    // start a transaction
    public async Task BeginAsync() {
         Console.WriteLine("Starting transaction");
        _transaction = await _context.Database.BeginTransactionAsync();

    }

    public async Task CommitAsync() {
        // if sucessfull it goes through else we rollback the transaction.

        try {
             Console.WriteLine("Commiting transaction");
             // relies on the db connection
             await _transaction!.CommitAsync();
        } catch {
            await RollBackAsync();
            // throw invalidException
            throw new InvalidOperationException("Transaction failed to commit.");
        }
       
    }

    // rollback a transaction.
    public async Task RollBackAsync(){
        foreach (var a in _context.ChangeTracker.Entries()) {
            switch (a.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
            }
        }
    }
}
