using Scentify.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Scentify.Repositories
{
    using Scentify.Data;

    public class ProductoRepository
    {
        public List<Producto> GetAll()
        {
            return MockDatabase.Productos;
        }

        public Producto? GetById(int id)
        {
            var product = MockDatabase.Productos.FirstOrDefault(p => p.ProductoID == id);
            if (product != null)
            {
                product.Resenas = MockDatabase.Resenas.Where(r => r.ProductoID == id).ToList();
            }
            return product;
        }

        public int Insert(Producto producto)
        {
            int newId = MockDatabase.Productos.Any() ? MockDatabase.Productos.Max(p => p.ProductoID) + 1 : 1;
            producto.ProductoID = newId;

            var category = MockDatabase.Categorias.FirstOrDefault(c => c.CategoriaID == producto.CategoriaID);
            producto.CategoriaNombre = category?.Nombre ?? "";

            var brand = MockDatabase.Marcas.FirstOrDefault(m => m.MarcaID == producto.MarcaID);
            producto.MarcaNombre = brand?.Nombre ?? "";

            MockDatabase.Productos.Add(producto);
            return 1;
        }

        public int Update(Producto producto)
        {
            var existing = MockDatabase.Productos.FirstOrDefault(p => p.ProductoID == producto.ProductoID);
            if (existing == null) return -1;

            existing.Nombre = producto.Nombre;
            existing.Descripcion = producto.Descripcion;
            existing.Precio = producto.Precio;
            existing.Stock = producto.Stock;
            existing.CategoriaID = producto.CategoriaID;
            existing.MarcaID = producto.MarcaID;
            existing.Genero = producto.Genero;
            existing.TamanioML = producto.TamanioML;
            existing.Descuento = producto.Descuento;
            existing.ImagenURL = producto.ImagenURL;

            var category = MockDatabase.Categorias.FirstOrDefault(c => c.CategoriaID == producto.CategoriaID);
            existing.CategoriaNombre = category?.Nombre ?? "";

            var brand = MockDatabase.Marcas.FirstOrDefault(m => m.MarcaID == producto.MarcaID);
            existing.MarcaNombre = brand?.Nombre ?? "";

            return 1;
        }

        public int Delete(int id)
        {
            var existing = MockDatabase.Productos.FirstOrDefault(p => p.ProductoID == id);
            if (existing == null) return -1;

            MockDatabase.Productos.Remove(existing);
            return 1;
        }

        public IEnumerable<Producto> GetFiltrados(string? marca, string? categoria, decimal? precioMin, decimal? precioMax, bool? soloStock, string? orden)
        {
            var query = MockDatabase.Productos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(marca))
            {
                query = query.Where(p => p.MarcaNombre != null && p.MarcaNombre.Equals(marca, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(categoria))
            {
                query = query.Where(p => p.CategoriaNombre != null && p.CategoriaNombre.Equals(categoria, StringComparison.OrdinalIgnoreCase));
            }

            if (precioMin.HasValue)
            {
                query = query.Where(p => p.Precio >= precioMin.Value);
            }

            if (precioMax.HasValue)
            {
                query = query.Where(p => p.Precio <= precioMax.Value);
            }

            if (soloStock.HasValue && soloStock.Value)
            {
                query = query.Where(p => p.Stock > 0);
            }

            if (!string.IsNullOrWhiteSpace(orden))
            {
                if (orden.Equals("precio_asc", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderBy(p => p.Precio);
                }
                else if (orden.Equals("precio_desc", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderByDescending(p => p.Precio);
                }
                else if (orden.Equals("nombre_asc", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderBy(p => p.Nombre);
                }
                else if (orden.Equals("nombre_desc", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderByDescending(p => p.Nombre);
                }
            }

            return query.ToList();
        }
    }
}
