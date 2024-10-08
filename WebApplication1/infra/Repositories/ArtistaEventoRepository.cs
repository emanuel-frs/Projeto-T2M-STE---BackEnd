﻿using Application.DTOs;
using Dapper;
using Domain.Entities;
using Domain.Projection;
using Domain.Repositories;
using Infrastructure.ConexaoDB;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infraestructore.Repositories
{
    public class ArtistaEventoRepository : IArtistaEventoRepository
    {
        private readonly PostgresDbConnection _postgresDbConnection;

        public ArtistaEventoRepository(PostgresDbConnection postgresDbConnection)
        {
            _postgresDbConnection = postgresDbConnection;
        }

        //LEMBRE DE MUDAR O CÓDIGO PARA FUNCIONAR
        //Create
        public async Task<int> AddArtistaEventoAsync(ArtistaEvento artistaEvento)
        {
            try
            {
                using (var dbConnection = _postgresDbConnection.CreateConnection())
                {
                    var sqlQuery = "INSERT INTO ArtistaEvento (eventoId, artistaId, dataRegistro) VALUES (@EventoId, @ArtistaId, @DataRegistro)";
                    return await dbConnection.ExecuteScalarAsync<int>(sqlQuery, artistaEvento);
                }
            }
            catch (Exception error)
            {
                throw new ApplicationException("Um erro aconteceu durante a query SQL: " + error);
            }
        }

        //GetById
        public async Task<ArtistaEvento> GetArtistaEventoByIdAsync(int id)
        {
            try
            {
                using (var dbConnection = _postgresDbConnection.CreateConnection())
                {
                    var sqlQuery = "SELECT * FROM ArtistaEvento WHERE ArtistaEventoId = @Id";
                    return await dbConnection.QueryFirstOrDefaultAsync<ArtistaEvento>(sqlQuery, new { Id = id });
                }
            }
            catch (Exception error)
            {
                throw new ApplicationException("Um erro aconteceu durante a query SQL: " + error);
            }
        }

        //GetAll
        public async Task<IEnumerable<IArtistaEventoProjection>> GetAllArtistaEventoAsync()
        {
            try
            {
                using (var dbConnection = _postgresDbConnection.CreateConnection())
                {
                    var sqlQuery = @"
                                    SELECT 
                                        e.nome AS nomeEvento,
                                        a.nome AS nomeArtista,
                                        CONCAT(en.rua, ', ', en.numero, ', ', en.cep) AS endereco,
                                        e.data AS dataEvento
                                    FROM 
                                        artistaevento ae
                                    JOIN 
                                        evento e ON ae.eventoid = e.eventoid
                                    JOIN 
                                        artista a ON ae.artistaid = a.artistaid
                                    JOIN 
                                        endereco en ON e.enderecoid = en.enderecoid;"
                                        ;

                    return await dbConnection.QueryAsync<ArtistaEventoDto>(sqlQuery);
                }
            }
            catch (Exception error)
            {
                throw new ApplicationException("Um erro aconteceu durante a query SQL: " + error.Message, error);
            }
        }


        //LEMBRE DE MUDAR O CÓDIGO PARA FUNCIONAR
        //Update
        public async Task UpdateArtistaEventoAsync(ArtistaEvento artistaEvento)
        {
            try
            {
                using (var dbConnection = _postgresDbConnection.CreateConnection())
                {
                    var sqlQuery = "UPDATE ArtistaEvento SET eventoId = @EventoId, artistaId = @ArtistaId, dataRegistro = @DataRegistro WHERE artistaEventoId = @ArtistaEventoId";
                    await dbConnection.ExecuteAsync(sqlQuery, artistaEvento);
                }
            }
            catch (Exception error)
            {
                throw new ApplicationException("Um erro aconteceu durante a query SQL: " + error);
            }
        }

        //Delete
        public async Task DeleteArtistaEventoAsync(int id)
        {
            try
            {
                using (var dbConnection = _postgresDbConnection.CreateConnection())
                {
                    var sqlQuery = "DELETE FROM ArtistaEvento WHERE ArtistaEventoId = @Id";
                    await dbConnection.ExecuteAsync(sqlQuery, new { Id = id });
                }
            }
            catch (Exception error)
            {
                throw new ApplicationException("Um erro aconteceu durante a query SQL: " + error);
            }
        }
    }
}
