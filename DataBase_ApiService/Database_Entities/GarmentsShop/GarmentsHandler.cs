using DataBaseEntityClasses.UsersMgt;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseEntityClasses.GarmentsShop
{
    public class GarmentsHandler
    {
        #region products
        public Product GetProduct(int id)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                return (from p in context.Products
                                 .Include(p => p.SubCategory.Category.Department)
                                 .Include(p => p.Fabric)
                                 .Include(p => p.Images)
                                 .Include(p => p.ColorsOffered)
                                 .Include(p => p.SizesOffered)
                        where p.Id == id
                        select p).First();
            }
        }


        public int GetProductsCount(string search) //will return the count of the total products against this search term
        {
            using (var context = new GarmentsContext())
            {
                if (string.IsNullOrEmpty(search) == false) //if search isnot null then return count of 
                {//only those records that matches the search term
                    return context.Products.Where(p => p.Name != null &&
                    p.Name.ToLower().Contains(search.ToLower())).Count();
                }
                else //if search is null then return count of all records
                {
                    return context.Products.Count();
                }
            }
        }

        public List<Product> GetProducts(string search, int pageNo)
        {
            //no of records this method will return on any call
            int PageSize = 3; 
            using (var context = new GarmentsContext())
            {

                if (string.IsNullOrEmpty(search) == false) //if some thing is passed in search then do this
                {   //if we have passed Search parameter means we are calling Action
                    //from Search button Click then take only records that are filtered according to pageSize and search string provided 
                    return context.Products.Where(p => p.Name != null &&  //for each Product of product table if p.Name!=null and the name of the product in LowerForm 
                    p.Name.ToLower().Contains(search.ToLower())).// contains search string passed in LowerForm then select all those products full filling these conditions  
                    OrderBy(x => x.Id).  //and order them by Id
                    Skip((pageNo - 1) * PageSize) //skip some records accrding to PageNo 
                    .Take(PageSize)   //take some records accrding to PageSize
                    .Include(p => p.SubCategory.Category.Department)
                    .Include(p => p.Fabric)
                    .Include(p => p.Images)
                    .Include(p => p.ColorsOffered)
                    .Include(p => p.SizesOffered)
                    .ToList(); //also incluse there related Category obj
                                                        //then return that List of Products 
                }
                else //if search is null then start taking all records
                {
                    //now accroding to this pageNo passed we need to get specific 10 records we can do this by using two methods Skip() and take()
                    // for first time or pageNo=1 skip 0 records and take first 10 when pageNo=2 is passed  skip first 10 and take
                    //next 10 records when pageNo=3 is passed  skip first 20 and take next 10 records  so we created a formula for Skip()..
                    //and Take() needs no formula as it will pick 10 allways 
                    return context.Products.OrderBy(x => x.Id).Skip((pageNo - 1) * PageSize).Take(PageSize).
                        Include(x => x.SubCategory)
                        .Include(p => p.SubCategory.Category.Department)
                        .Include(p => p.Fabric)
                        .Include(p => p.Images)
                        .Include(p => p.ColorsOffered)
                        .Include(p => p.SizesOffered)
                        .ToList();
                    //if we don,t use take() it will pick all the next records after skiping some.
                    //for using skip() the List/Records/Products should be sorted so used OrderBy(x=>x.Id) before it..
                }
            }
        }


        public List<Banner> GetBanners()
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                return (from p in context.Banners
                        select p).ToList();
            }
        }

        public Banner GetBanner(int id)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                return (from p in context.Banners
                        where p.Id == id
                        select p).First();
            }
        }


        public List<Product> GetProducts()
        {
            using (GarmentsContext context = new GarmentsContext())
            {

                try
                {
                    return (from p in context.Products
                                     .Include(p => p.SubCategory.Category.Department)
                                     .Include(p => p.Fabric)
                                     .Include(p => p.Images)
                                     .Include(p => p.ColorsOffered)
                                     .Include(p => p.SizesOffered)

                            select p).ToList();
                }
                catch (Exception ex)
                {
                    var c = ex.Message;
                    return null;                }
            }
        }


        public void AddProduct(Product product)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                context.Entry(product.SubCategory).State = EntityState.Unchanged;
                context.Entry(product.Fabric).State = EntityState.Unchanged;
                foreach (var c in product.ColorsOffered)
                {
                    context.Entry(c).State = EntityState.Unchanged;
                }
                foreach (var s in product.SizesOffered)
                {
                    context.Entry(s).State = EntityState.Unchanged;
                }
                context.Products.Add(product);
                context.SaveChanges();
            }
        }


        public void UpdateProduct(int id, Product model)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                Product p = new Product();
                p = context.Products.Find(id);
                if (!string.IsNullOrWhiteSpace(model.Name)) p.Name = model.Name;

                p.Id = model.Id;
                p.Name = model.Name;
                p.SubCategory = model.SubCategory;
                p.Price = model.Price;
                p.Description = model.Description;
                p.LaunchingDate = model.LaunchingDate;
                p.Fabric = model.Fabric;
                p.ColorsOffered = model.ColorsOffered;
                p.SizesOffered = model.SizesOffered;
                p.Images = model.Images;

                //context.Entry(model.Images).State = EntityState.Unchanged;
                //context.Entry(model.SubCategory).State = EntityState.Unchanged;
                //context.Entry(model.SizesOffered).State = EntityState.Unchanged;
                context.SaveChanges();
            }
        }


        #endregion


        public int GetProductsCount(int catId)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                return context.Products.Where(x => x.SubCategory.Category.Id == catId).Count();
            }
        }

        public List<Product> GetProductsbyCategory(int CatId)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                var c = (from p in context.Products
                                     .Include(p => p.SubCategory.Category.Department)
                                     .Include(p => p.Images)
                                     .Where(x => x.SubCategory.Category.Id == CatId)
                                      select p).ToList();
                return c ;
            }
        }


        #region categories

        public List<Category> GetCategories()
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                return (from c in context.Categories
                            .Include(c => c.Department)
                        select c).ToList();
            }
        }

        public List<Category> GetCategories(int deptId)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                return (from c in context.Categories
                            .Include(c => c.Department)
                            where c.Department.Id == deptId
                        select c).ToList();
            }
        }


        public Category GetCategory(int id)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                Category found = (from c in context.Categories
                                  .Include(c => c.Department)
                                  where c.Id == id
                                  select c).FirstOrDefault();
                return found;
            }

        }


        public List<SubCategory> GetSubCategories()
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                return (from c in context.SubCategories
                            .Include(c => c.Category)
                        select c).ToList();
            }
        }

        public List<SubCategory> GetSubCategories(int catId)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                return (from c in context.SubCategories
                            .Include(c => c.Category.Department)
                        where c.Category.Id == catId
                        select c).ToList();
            }
        }

        public SubCategory GetSubCategory(int id)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                SubCategory found = (from c in context.SubCategories
                                  .Include(c => c.Category)
                                  where c.Id == id
                                  select c).FirstOrDefault();
                return found;
            }

        }


        public void AddCategory(Category category)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                context.Entry(category.Department).State = EntityState.Unchanged;
                context.Categories.Add(category);
                context.SaveChanges();
            }
        }


        public void AddOrder(Order order)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                try {
                    //error : The entity type List`1 is not part of the model for the current context.
                    //solution we are passing List here context.Entry(productList).State = EntityState.Unchanged;
                    //but we should pass one product at a time...
                    foreach (var product in order.Products)
                    {
                        context.Entry(product.SubCategory).State = EntityState.Unchanged;
                        context.Entry(product.Fabric).State = EntityState.Unchanged;
                        foreach (var c in product.ColorsOffered)
                        {
                            context.Entry(c).State = EntityState.Unchanged;
                        }
                        foreach (var s in product.SizesOffered)
                        {
                            context.Entry(s).State = EntityState.Unchanged;
                        }
                        foreach (var c in product.Images)
                        {
                            context.Entry(c).State = EntityState.Unchanged;
                        }
                        context.Entry(product).State = EntityState.Unchanged;
                    }
                    context.Entry(order.User).State = EntityState.Unchanged;
                    context.Orders.Add(order);
                    context.SaveChanges();
                }
                catch (Exception ex) {
                    var a = ex.Message;
                }
            }
        }


        public void UpdateCategory(int id, Category category)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                Category found = context.Categories.Find(id);
                if (!string.IsNullOrWhiteSpace(category.Name)) found.Name = category.Name;
                if (!string.IsNullOrWhiteSpace(category.ImageUrl)) found.ImageUrl = category.ImageUrl;
                if (category.Department != null && category.Department.Id > 0) found.Department = category.Department;

                context.Entry(category.Department).State = EntityState.Unchanged;
                context.SaveChanges();
            }
        }

        public void DeleteCategory(int id)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                Category found = context.Categories.Find(id);
                context.Categories.Remove(found);
                context.SaveChanges();
            }
        }

        public void DeleteProduct(int id)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                Product found = context.Products.Where(x => x.Id == id).FirstOrDefault();
                //First Delete Images which are connected to it as both images each image is created for a specific product
                //so first we need to delete so for this First In ProductImage table we don,t had the Product obj so we created
                //there so now we can check which image is connected to which product then we applied query to find images
                //related to the product which we wanted to delete then Delete the Product...
                //but why we did not deleted Subcategory and other as productTable has foreignKey of Subcategory so when product deleted
                //there refernce also removes but incase of Images as ProductImages Table has foreignKey for Product table so first delete
                //Image then Product
                List<ProductImage> Images = context.ProductImages.Where(x => x.product.Id == found.Id).ToList();

                context.ProductImages.RemoveRange(Images);
                context.Products.Remove(found);
                context.SaveChanges();
            }
        }
        #endregion
        
        #region departments
        public Department GetDeparment(int id)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                return (from d in context.Departments
                        where d.Id == id
                        select d).First();
            }
        }
        public List<Department> GetDeparments()
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                return (from d in context.Departments
                        select d).ToList();
            }
        }

        public void AddDepartment(Department dept)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                context.Departments.Add(dept);
                context.SaveChanges();
            }
        }

        public void UpdateDepartment(int idToSearch, Department dept)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                Department found = (from d in context.Departments
                                    where d.Id == idToSearch
                                    select d).First();
                if ((!string.IsNullOrWhiteSpace(dept.Name))) found.Name = dept.Name;
                if ((!string.IsNullOrWhiteSpace(dept.ImageUrl))) found.ImageUrl = dept.ImageUrl;
                context.SaveChanges();
            }
        }

        public void DeleteDepartment(int idToSearch)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                Department found = (from d in context.Departments
                                    where d.Id == idToSearch
                                    select d).First();

                context.Departments.Remove(found);
                context.SaveChanges();
            }
        }
        #endregion

        public List<Color> GetColors()
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                return (from c in context.Colors select c).ToList();
            }
        }

        public Color GetColor(int id)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                return (from d in context.Colors
                        where d.Id == id
                        select d).First();
            }
        }


        public List<Role> GetRoles()
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                return (from c in context.Roles select c).ToList();
            }
        }

        public Role GetRole(int id)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                return (from d in context.Roles
                        where d.Id == id
                        select d).First();
            }
        }


        public List<Size> GetSizes()
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                return (from s in context.Sizes select s).ToList();
            }
        }

        public Size GetSize(int id)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                return (from d in context.Sizes
                        where d.Id == id
                        select d).First();
            }
        }

        public List<Fabric> GetFabrics()
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                return (from f in context.Fabrics select f).ToList();
            }
        }

        public Fabric GetFabric(int id)
        {
            using (GarmentsContext context = new GarmentsContext())
            {
                return (from d in context.Fabrics
                        where d.Id == id
                        select d).First();
            }
        }

    }
}
