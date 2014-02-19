--------------------------------------------------------------------------------
           Notice about RIA Services and Entity Framework Code First
--------------------------------------------------------------------------------
You have installed the RIAServices.EntityFramework package.  This package
provides a class called DbDomainService<T>, which enables you to create
DomainService classes based on EntityFramework Code First DbContext classes.

GETTING STARTED

1. Create a DbContext class, following guidance from:
   http://msdn.microsoft.com/data/ef

2. Create a DbDomainService<T> class, using your DbContext type for T.  This
   class is in the System.ServiceModel.DomainServices.EntityFramework namespace,
   from the Microsoft.ServiceModel.DomainServices.EntityFramework assembly.

   If you are migrating from EntityFramework 4.0, your existing DomainService
   classes derive from LinqToEntitiesDomainService<T>, and this will need to
   be changed to a DbDomainService<T> after you create a new DbContext class
   to use.  You cannot use DbDomainService with an ObjectContext.

   Note that migrating from an ObjectContext-based entity model to a
   DbContext model will require significant changes both to your model as well
   as changes to your Domain Service class.

   When you are no longer using LinqToEntitiesDomainService<T>, you can
   remove your reference to System.ServiceModel.DomainServices.EntityFramework;
   DbDomainService<T> is in Microsoft.ServiceModel.DomainServices.EntityFramework.

   If you wish to continue using ObjectContext-based entity models, consider
   the RIAServices.EF4 NuGet package instead of RIAServices.EntityFramework.

3. Follow the same patterns as any other DomainService class, creating
   Get, Update, Insert, and Delete methods for the entity types needed.

4. To access your DbContext from within the DbDomainService<T> class, you
   will use 'this.DbContext' (or 'Me.DbContext' in VB), instead of the
   ObjectContext property you may have used with Entity Framework 4.0.


KNOWN ISSUES

During installation of this NuGet package, some users have experienced
problems with their web.config files, where the installation results in
duplicated or erroneous configuration entries.

1. Your application is unable to load the EntityFramework assembly,
   reporting an error that the assembly cannot be found.

   This is often caused by an incorrect binding redirect that
   specifies version "4.1.0.0" for EntityFramework, when version
   "4.4.0.0" or greater is needed.

   You may need to add or correct binding redirects in your web.config
   file.  See BINDING REDIRECTS below.

   If you update the EntityFramework NuGet package in your application
   to the latest version, the binding redirects should be corrected
   automatically.

2. A <configSections> element is added to your web.config file, but
   in the wrong location.  The <configSections> element must be
   at the top of the web.config file, as the first element within
   the <configuration> element.

   If this has occurred, you will see an error stating:
   "The requested page cannot be accessed because the related
   configuration data for the page is invalid."

   To correct this, see CONFIG SECTION ELEMENT below.

3. Installing RIAServices.EntityFramework into a WCF RIA Services Class Library
   causes design-time discovery of entity types to fail (silently).

   When installing this package into a WCF RIA Services Class Library, a
   web.config file is added to your project. This file is used by the
   Add New Doman Service dialog and during RIA client proxy class generation.
   Your connection strings have been copied into this file. For consistent
   behavior between runtime and design-time, please keep it up-to-date.

      IMPORTANT:
      If you update the EntityFramework package, delete the web.config file and
      uninstall and reinstall this package to ensure continued design-time support.

   If your project already included a Web.config file, it was not been modified
   and you ill need to copy your connection strings into a web.config file within
   the class library project manually.



BINDING REDIRECTS

If you need to add or correct binding redirects in your application because
the EntityFramework assembly cannot be found, you can use the following as
an example for what to add to your web.config file.

<configuration>
  <runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="EntityFramework" publicKeyToken="b77a5c561934e089" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-4.4.0.0" newVersion="4.4.0.0" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
</configuration>

Note that the upper-bound of the "oldVersion" and the "newVersion" should
have the same value and that they should match the Assembly Version of the
EntityFramework.dll that is being used by your application.

For more information on Binding Redirects, see:
http://msdn.microsoft.com/library/eftw1fys.aspx


CONFIG SECTIONS ELEMENT

If the installation of this package resulted in a <configSections> element that
isn't at the top of the <configuration> element in your web.config, then you will
need to move the <configSections> element to be the first child under
<configuration>.

Here is the <configSections> block that needs to exist:

<configuration>
  <configSections>
    <section name="entityFramework" type="System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
  </configSections>
  <entityFramework>
    <defaultConnectionFactory type="System.Data.Entity.Infrastructure.SqlConnectionFactory, EntityFramework" />
  </entityFramework>
</configuration>

Note that the Version of the EntityFramework assembly will need to match the Assembly for
the version of EntityFramework that you have installed.


